using G1ANT.Browser.Driver.Data;
using System;
using System.ServiceModel;
using G1ANT.Browser.Driver.Interfaces;
using G1ANT.Browser.Driver.Actions;
using Newtonsoft.Json;

namespace G1ANT.Browser.Driver.Services
{
    public abstract class BrowserClient :
        BrowserClientBase<IBrowserAction>,
        IBrowserDriver,
        IBrowserActionCallback
    {
        public BrowserService BrowserService { get; }

        public BrowserClient(BrowserService browserService)
        {
            BrowserService = browserService;
        }

        protected ActionResponse Execute(ActionBase command)
        {
            if (command.Timeout.TotalMilliseconds == 0)
                command.Timeout = BrowserService.DefaultTimeout;
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
                var pipeProxy = CreateChannel(BrowserService.DefaultTimeout);
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

        public abstract void OpenBrowserWithUrl(string url, string extraArguments = "");

        protected virtual IDisposable CreateExtensionChecker()
        {
            return null;
        }

        protected string ProcessAction(ActionBase action)
        {
            using (var checker = CreateExtensionChecker())
            {
                try
                {
                    var response = Execute(action);
                    if (response.Succeedded)
                        return response.JsonData;

                    if (response.JsonData != null)
                    {
                        var data = JsonConvert.DeserializeObject<ErrorResult>(response.JsonData);
                        throw new Exception(data.Error);
                    }
                    else
                        throw new SystemException();
                }
                catch (EndpointNotFoundException)
                {
                    return ProcessAction(action);
                }
                catch
                {
                    throw;
                }
            }
        }

        public BrowserTab ActivateTab(ActivateTabAction action)
        {
            var result = ProcessAction(action);
            return JsonConvert.DeserializeObject<BrowserTab>(result);
        }

        public BrowserTab FindTab(FindTabAction action)
        {
            var result = ProcessAction(action);
            return JsonConvert.DeserializeObject<BrowserTab>(result);
        }

        public void Click(ClickAction action)
        {
            ProcessAction(action);
        }

        public void CloseTab(CloseTabAction action)
        {
            ProcessAction(action);
        }

        public BrowserTab GetActiveTab(GetActiveTabAction action)
        {
            var result = ProcessAction(action);
            return JsonConvert.DeserializeObject<BrowserTab>(result);
        }

        public string GetAttribute(GetAttributeAction action)
        {
            var result = ProcessAction(action);
            var model = JsonConvert.DeserializeObject<ValueModel>(result);
            return model.Value;
        }

        public string GetHtml(GetHtmlAction action)
        {
            var result = ProcessAction(action);
            var model = JsonConvert.DeserializeObject<ValueModel>(result);
            return model.Value;
        }

        public string GetOuterHtml(GetOuterHtmlAction action)
        {
            var result = ProcessAction(action);
            var model = JsonConvert.DeserializeObject<ValueModel>(result);
            return model.Value;
        }

        public string GetInnerHtml(GetInnerHtmlAction action)
        {
            var result = ProcessAction(action);
            var model = JsonConvert.DeserializeObject<ValueModel>(result);
            return model.Value;
        }

        public string GetText(GetTextAction action)
        {
            var result = ProcessAction(action);
            var model = JsonConvert.DeserializeObject<ValueModel>(result);
            return model.Value;
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
            ProcessAction(action);
        }

        public BrowserTab SetUrl(SetUrlAction action)
        {
            var result = ProcessAction(action);
            return JsonConvert.DeserializeObject<BrowserTab>(result);
        }

        public void TypeText(TypeTextAction action)
        {
            ProcessAction(action);
        }

        public void PressKey(PressKeyAction action)
        {
            ProcessAction(action);
        }
    }
}
