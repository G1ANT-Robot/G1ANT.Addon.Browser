using G1ANT.Browser.Driver.Actions;
using G1ANT.Browser.Driver.Data;
using G1ANT.Browser.Driver.Services;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace G1ANT.Chrome.Driver
{
    public class ChromeClient : BrowserClient
    {
        protected override string ServerName => Configuration.ServerName;

        public ChromeClient() : base(new ChromeEventsServer(new BrowserEventsService()))
        {
        }

        private bool urlLoaded = false;
        protected override void OpenBrowserWithUrl(string url)
        {
            urlLoaded = false;
            EventService.OnTabUpdated += TabUpdatedHandler;

            Process.Start("chrome.exe", $"\"{url}\" --new-window");

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

            EventService.OnTabUpdated -= TabUpdatedHandler;
            throw new TimeoutException();
        }

        protected void TabUpdatedHandler(BrowserTab tab)
        {
            if (tab.Status == "complete")
                urlLoaded = true;
        }

        public override BrowserTab Open(OpenAction action)
        {
            CheckExtension();

            OpenBrowserWithUrl(action.Url);

            return null;
        }
    }
}
