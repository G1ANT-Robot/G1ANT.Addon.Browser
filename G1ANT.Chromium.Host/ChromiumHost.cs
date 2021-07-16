using G1ANT.Chromium.Host.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace G1ANT.Chromium.Host
{
    public abstract class ChromiumHost
    {
        private readonly bool sendConfirmationReceipt;
        private readonly string manifestPath;
        private bool running = false;
        private bool waitingForInput = false;
        private byte[] inputStreamBuffer = new byte[4];
        private BlockingCollection<ChromiumCommandResponse> commandResponses = new BlockingCollection<ChromiumCommandResponse>();
        private AutoResetEvent messageReceived;

        public abstract string Hostname { get; }

        public delegate void EventTriggeredHandler(ChromiumBrowserEvent browserEvent);
        public event EventTriggeredHandler EventTriggered;

        public List<ChromiumBrowserRegistrar> SupportedBrowsers { get; }

        public ChromiumHost(bool sendConfirmationReceipt = true)
        {
            SupportedBrowsers = new List<ChromiumBrowserRegistrar>();

            this.sendConfirmationReceipt = sendConfirmationReceipt;
            manifestPath = Path.Combine(Utils.AssemblyLoadDirectory(), Hostname + "-manifest.json");

            messageReceived = new AutoResetEvent(false);
        }

        public void Listen()
        {
            if (!IsRegistered())
                throw new ApplicationException(Hostname);

            running = true;
            while (running)
            {
                messageReceived.Reset();
                ReadAsync();
                messageReceived.WaitOne();
            }
        }

        public abstract void ExitExtension();

        private void ReadAsync()
        {
            if (waitingForInput)
                return;

            Stream stdin = Console.OpenStandardInput();

            waitingForInput = true;
            stdin.BeginRead(inputStreamBuffer, 0, 4, new AsyncCallback(EndReadAsync), stdin);
        }

        private bool IsExtensionDisconnected(char[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                return true;
            else
                return buffer.All(item => item == 0);
        }

        private void EndReadAsync(IAsyncResult asyncResult)
        {
            Stream stdin = asyncResult.AsyncState as Stream;
            stdin.EndRead(asyncResult);

            char[] buffer = new char[BitConverter.ToInt32(inputStreamBuffer, 0)];

            using (StreamReader reader = new StreamReader(stdin))
                while (reader.Peek() >= 0)
                    reader.Read(buffer, 0, buffer.Length);

            if (IsExtensionDisconnected(buffer))
                ExitExtension();

            var data = JsonConvert.DeserializeObject<JObject>(new string(buffer));
            ProcessRequestData(data);

            waitingForInput = false;
            messageReceived.Set();
        }


        protected void ProcessRequestData(JObject data)
        {
            if (sendConfirmationReceipt)
                SendMessage(new ChromiumResponseConfirmation(data).GetJObject());
            ProcessReceivedMessage(data);
        }

        public void Stop()
        {
            running = false;
            messageReceived.Set();
        }

        protected virtual void ProcessReceivedMessage(JObject data)
        {
            try
            {
                if (data != null)
                {
                    var message = data.ToObject<ChromiumMessage>();
                    switch (message.Message)
                    {
                        case ChromiumCommandResponse.MessageName:
                            {
                                var commandResponse = message.Data.ToObject<ChromiumCommandResponse>();
                                commandResponses.Add(commandResponse);
                            }
                            break;
                        case ChromiumBrowserEvent.MessageName:
                            if (EventTriggered != null)
                            {
                                var browserEvent = message.Data.ToObject<ChromiumBrowserEvent>();
                                EventTriggered(browserEvent);
                            }
                            break;
                    }
                }
            }
            catch
            {
            }
        }

        public virtual void SendMessage(JObject data)
        {
            SendMessage(data.ToString(Formatting.None));
        }
        public virtual void SendMessage(string data)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
            Stream stdout = Console.OpenStandardOutput();
            stdout.WriteByte((byte)((bytes.Length >> 0) & 0xFF));
            stdout.WriteByte((byte)((bytes.Length >> 8) & 0xFF));
            stdout.WriteByte((byte)((bytes.Length >> 16) & 0xFF));
            stdout.WriteByte((byte)((bytes.Length >> 24) & 0xFF));
            stdout.Write(bytes, 0, bytes.Length);
            stdout.Flush();
        }

        public void GenerateManifest(string description, string[] allowedOrigins, bool overwrite = false)
        {
            if (!File.Exists(manifestPath) || overwrite)
            {
                string manifest = JsonConvert.SerializeObject(new ChromiumManifest(Hostname, description, Utils.AssemblyExecutablePath(), allowedOrigins));
                File.WriteAllText(manifestPath, manifest);
            }
        }

        public void RemoveManifest()
        {
            if (File.Exists(manifestPath))
            {
                File.Delete(manifestPath);
            }
        }

        public bool IsRegistered()
        {
            bool result = false;

            foreach (var browser in SupportedBrowsers)
            {
                result = result || browser.IsRegistered(Hostname, manifestPath);
            }

            return result;
        }

        public void Register()
        {
            foreach (var browser in SupportedBrowsers)
            {
                browser.Register(Hostname, manifestPath);
            }
        }

        public void Unregister()
        {
            foreach (var browser in SupportedBrowsers)
            {
                browser.Unregister(Hostname);
            }
        }

        public ChromiumCommandResponse WaitForCommandResponse(string id, int timeout = 60000)
        {
            long start = Environment.TickCount;

            do
            {
                var response = commandResponses.FirstOrDefault(x => x.Id == id);
                if (response != null)
                    return response;
            }
            while (Math.Abs(Environment.TickCount - start) < timeout);
            return null;
        }

    }
}
