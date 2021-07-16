using G1ANT.Browser.Driver.Data;
using System;
using System.ServiceModel;
using G1ANT.Browser.Driver.Extensions;
using G1ANT.Browser.Driver.Interfaces;
using G1ANT.Browser.Driver.Actions;
using Newtonsoft.Json;
using System.Threading;

namespace G1ANT.Browser.Driver.Services
{
    public abstract class BrowserClient :
        BrowserClientBase<IBrowserAction>,
        IBrowserDriver,
        IBrowserActionCallback
    {
        static private DriverServer<BrowserEventsService, IBrowserEvents> s_eventsServer = null;

        public BrowserEventsService EventService { get => s_eventsServer?.Service; }

        public BrowserClient(DriverServer<BrowserEventsService, IBrowserEvents> eventsServer)
        {
            if (s_eventsServer == null)
            {
                s_eventsServer = eventsServer;
                s_eventsServer.Start();
            }
        }

        protected ActionResponse Execute(ActionBase command)
        {
            var pipeProxy = CreateChannel(command.Timeout);
            var response = pipeProxy.Execute(command);
            return response;
        }

        protected void ExecuteAsync(ActionBase command)
        {
            var pipeProxy = CreateChannel(command.Timeout);
            pipeProxy.ExecuteAsync(command);
        }

        public bool IsExtensionConnected()
        {
            try
            {
                var pipeProxy = CreateChannel(10000);
                return pipeProxy.IsConnected();
            }
            catch (EndpointNotFoundException)
            {
                return false;
            }
            catch
            {
                throw;
            }
        }

        public void CommandFinished(ActionResponse response)
        {
            throw new NotImplementedException();
        }

        protected abstract void OpenBrowserWithUrl(string url);

        protected void StartBrowserExtension()
        {
            try
            {
                OpenBrowserWithUrl(@"file://mac/Home/Documents/G1ANT.Robot/Add-on/G1ANT.Addon.Browser.Reconnecting.Extension.html");
            }
            catch (TimeoutException ex)
            {
                if (!IsExtensionConnected())
                    throw ex;
            }
            catch
            {
                throw;
            }
        }

        public void CheckExtension()
        {
            if (!IsExtensionConnected())
            {
                StartBrowserExtension();
            }
        }

        protected string ProcessAction(ActionBase action)
        {
            try
            {
                CheckExtension();
                var response = Execute(action);
                if (response.Succeedded)
                    return response.JsonData;
                try
                {
                    var data = JsonConvert.DeserializeObject<ErrorResult>(response.JsonData);
                    throw new Exception(data.Error);
                }
                catch
                {
                    throw new SystemException();
                }
            }
            catch (EndpointNotFoundException)
            {
                StartBrowserExtension();
                return ProcessAction(action);
            }
            catch
            {
                throw;
            }
        }

        public BrowserTab ActivateTab(ActivateTabAction action)
        {
            var result = ProcessAction(action);
            return JsonConvert.DeserializeObject<BrowserTab>(result);
        }

        public void Click(ClickAction action)
        {
            throw new NotImplementedException();
        }

        public void Close(CloseTabAction action)
        {
            throw new NotImplementedException();
        }

        public BrowserTab GetActiveTab(GetActiveTabAction action)
        {
            var result = ProcessAction(action);
            return JsonConvert.DeserializeObject<BrowserTab>(result);
        }

        public string GetAttribute(GetAttributeAction action)
        {
            throw new NotImplementedException();
        }

        public string GetHtml(GetHtmlAction action)
        {
            throw new NotImplementedException();
        }

        public string GetOuterHtml(GetOuterHtmlAction action)
        {
            throw new NotImplementedException();
        }

        public string GetText(GetTextAction action)
        {
            throw new NotImplementedException();
        }

        public BrowserTab NewTab(NewTabAction action)
        {
            var result = ProcessAction(action);
            return JsonConvert.DeserializeObject<BrowserTab>(result);
        }

        public abstract BrowserTab Open(OpenAction action);

        public BrowserTab Refresh(RefreshAction action)
        {
            var result = ProcessAction(action);
            return JsonConvert.DeserializeObject<BrowserTab>(result);
        }

        public void SetAttribute(SetAttributeAction action)
        {
            throw new NotImplementedException();
        }

        public BrowserTab SetUrl(SetUrlAction action)
        {
            var result = ProcessAction(action);
            return JsonConvert.DeserializeObject<BrowserTab>(result);
        }
    }
}
