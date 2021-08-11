using G1ANT.Browser.Driver.Data;
using G1ANT.Chrome.Driver;
using G1ANT.Chromium.Host;
using G1ANT.Chromium.Host.Data;
using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace G1ANT.Chrome.Host
{
    public class ChromeHostWorker : ApplicationContext
    {
        private const string RegisterArgument = "register";
        private static readonly string[] HostAllowedOrigins = new string[] { "chrome-extension://iiojoficinkdiloplfipdjegidpebhfn/" };
        private const string HostDescription = "G1ANT.Studio host for browser interaction";
        protected Thread hostThread;
        protected readonly ChromiumHost host;
        protected readonly ChromeServer<ChromeActionService> server;
        protected readonly ChromeEventsClient eventsClient;

        public ChromeHostWorker(
            ChromeHost host,
            ChromeServer<ChromeActionService> server,
            ChromeEventsClient eventsClient)
        {
            this.host = host;
            this.server = server;
            this.eventsClient = eventsClient;
        }

        public void Run(string[] args)
        {
            if (NeedsRegistrattion(args))
                RegisterHost();
            else
            {
                RunServices();
                Application.Run(this);
            }
        }

        protected override void ExitThreadCore()
        {
            StopServices();
            base.ExitThreadCore();
        }

        protected void StopServices()
        {
            host.EventTriggered -= OnBrowserEventTriggered;
            host.Stop();
            server.Stop();
        }

        protected void RunServices()
        {
            host.EventTriggered += OnBrowserEventTriggered;
            hostThread = new Thread(new ThreadStart(host.Listen));
            hostThread.Start();

            server.Start();
        }

        protected void OnBrowserEventTriggered(ChromiumBrowserEvent browserEvent)
        {
            try
            {
                switch (browserEvent.Event)
                {
                    case "runtime.onStartup":
                        break;
                    case "runtime.onSuspend":
                        break;
                    case "extension.connected":
                        eventsClient.ExtensionConnected();
                        break;
                    case "extension.disconnected":
                        eventsClient.ExtensionDisconnected();
                        break;
                    case "tabs.onCreated":
                        eventsClient.TabCreated(browserEvent.Data.ToObject<BrowserTab>());
                        break;
                    case "tabs.onUpdated":
                        eventsClient.TabUpdated(browserEvent.Data.ToObject<BrowserTab>());
                        break;
                }
            }
            catch (Exception ex)
            {
                var qq = ex;
            }
        }

        protected void RegisterHost()
        {
            ChromeHost host = new ChromeHost();
            host.GenerateManifest(HostDescription, HostAllowedOrigins, true);
            host.Register();
        }

        protected bool NeedsRegistrattion(string[] args)
        {
            if (args.Length == 1)
            {
                var param = args[0].ToLower().Remove(0, 1);
                if (param == RegisterArgument)
                    return true;
            }
            return false;
        }

    }
}
