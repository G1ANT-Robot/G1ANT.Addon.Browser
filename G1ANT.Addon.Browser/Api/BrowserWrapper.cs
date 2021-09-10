using G1ANT.Browser.Driver.Actions;
using G1ANT.Browser.Driver.Data;
using G1ANT.Browser.Driver.Interfaces;
using System;

namespace G1ANT.Addon.Browser.Api
{
    public class BrowserWrapper
    {
        private static int _idcounter = 0;

        public IBrowserDriver Driver { get; private set; }

        public int Id { get; private set; }

        public BrowserWrapper(IBrowserDriver driver)
        {
            Id = ++_idcounter;
            this.Driver = driver;
        }

        public void OpenUrl(string url, TimeSpan timeout, bool noWait)
        {
            var action = new OpenAction()
            {
                Timeout = timeout,
                Url = url,
                NoWait = noWait
            };
            Driver.Open(action);
        }

        public void SetUrl(string url, TimeSpan timeout, bool noWait)
        {
            var action = new SetUrlAction()
            {
                Timeout = timeout,
                Url = url,
                NoWait = noWait
            };
            Driver.SetUrl(action);
        }

        public void Refresh(TimeSpan timeout)
        {
            var action = new RefreshAction()
            {
                BypassCache = false,
                Timeout = timeout
            };
            Driver.Refresh(action);
        }

        public void CloseTab(TimeSpan timeout)
        {
            var action = new CloseTabAction()
            {
                Timeout = timeout
            };
            Driver.CloseTab(action);
        }

        public void NewTab(string url, bool noWait, TimeSpan timeout)
        {
            var action = new NewTabAction()
            {
                Timeout = timeout,
                Url = url,
                NoWait = noWait
            };
            Driver.NewTab(action);
        }

        public void ActivateTab(string phrase, string by, TimeSpan timeout)
        {
            var action = new ActivateTabAction()
            {
                Search = phrase,
                By = by,
                Timeout = timeout
            };
            Driver.ActivateTab(action);
        }

        public void Click(BrowserCommandArguments search, TimeSpan timeout, bool waitForNewWindow = false)
        {
            var action = new ClickAction()
            {
                Search = search.Search.Value,
                By = search.By.Value,
                Timeout = timeout
            };
            Driver.Click(action);
        }

        public void TypeText(BrowserCommandArguments search, string text, TimeSpan timeout)
        {
            var action = new TypeTextAction()
            {
                Search = search.Search.Value,
                By = search.By.Value,
                Text = text,
                Timeout = timeout
            };
            Driver.TypeText(action);
        }

        public void PressKey(BrowserCommandArguments search, string keyText, TimeSpan timeout)
        {
            var action = new PressKeyAction()
            {
                Search = search.Search.Value,
                By = search.By.Value,
                Key = keyText,
                Timeout = timeout
            };
            Driver.PressKey(action);
        }

        public void SetAttributeValue(BrowserCommandArguments search, string attributeName, string attributeValue, TimeSpan timeout)
        {
            var action = new SetAttributeAction()
            {
                Search = search.Search.Value,
                By = search.By.Value,
                Name = attributeName,
                Value = attributeValue,
                Timeout = timeout
            };
            Driver.SetAttribute(action);
        }

        public string GetAttributeValue(BrowserCommandArguments search, string attributeName, TimeSpan timeout)
        {
            var action = new GetAttributeAction()
            {
                Search = search.Search.Value,
                By = search.By.Value,
                Name = attributeName,
                Timeout = timeout
            };
            return Driver.GetAttribute(action);
        }

        public string GetHtml(BrowserIFrameArguments search, TimeSpan timeout)
        {
            var action = new GetHtmlAction()
            {
                Timeout = timeout
            };
            return Driver.GetHtml(action);
        }

        public string GetInnerHtml(BrowserCommandArguments search, TimeSpan timeout)
        {
            var action = new GetInnerHtmlAction()
            {
                Search = search.Search.Value,
                By = search.By.Value,
                Timeout = timeout
            };
            return Driver.GetInnerHtml(action);
        }

        public string GetOuterHtml(BrowserCommandArguments search, TimeSpan timeout)
        {
            var action = new GetOuterHtmlAction()
            {
                Search = search.Search.Value,
                By = search.By.Value,
                Timeout = timeout
            };
            return Driver.GetOuterHtml(action);
        }

        public string GetTextValue(BrowserCommandArguments search, TimeSpan timeout)
        {
            var action = new GetTextAction()
            {
                Search = search.Search.Value,
                By = search.By.Value,
                Timeout = timeout
            };
            return Driver.GetText(action);
        }

        public BrowserTab GetActiveTab(TimeSpan timeout)
        {
            var action = new GetActiveTabAction()
            {
                Timeout = timeout
            };
            return Driver.GetActiveTab(action);
        }
    }
}
