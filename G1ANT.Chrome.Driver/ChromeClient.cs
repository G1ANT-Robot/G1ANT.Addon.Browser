using G1ANT.Browser.Driver.Actions;
using G1ANT.Browser.Driver.Data;
using G1ANT.Browser.Driver.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace G1ANT.Chrome.Driver
{
    public class ChromeClient : BrowserClient
    {
        protected override string ServerName => Configuration.ServerName;

        public ChromeClient(BrowserService browserService) : base(browserService)
        {
        }

        private bool urlLoaded = false;
        public override void OpenBrowserWithUrl(string url, string extraArguments = "")
        {
            urlLoaded = false;
            BrowserService.EventsService.OnTabUpdated += TabUpdatedHandler;

            Process.Start("chrome.exe", $"\"{url}\" {extraArguments}");

            long start = Environment.TickCount;
            new Task(() => {
                do
                {
                    if (urlLoaded)
                        return;
                    Application.DoEvents();
                }
                while (Math.Abs(Environment.TickCount - start) < 20000);
            }).RunSynchronously();
            BrowserService.EventsService.OnTabUpdated -= TabUpdatedHandler;
            if (!urlLoaded)
                throw new TimeoutException();
        }

        protected void TabUpdatedHandler(BrowserTab tab)
        {
            if (tab.Status == "complete")
                urlLoaded = true;
        }

        public override BrowserTab Open(OpenAction action)
        {
            using (var checker = CreateExtensionChecker())
            {
                OpenBrowserWithUrl(action.Url, ChromeArguments.NewWindwow);
                return null;
            }
        }

        protected override IDisposable CreateExtensionChecker()
        {
            if (IsExtensionConnected())
                return null;

            return new ChromeExtensionChecker(this);
        }
    }
}
