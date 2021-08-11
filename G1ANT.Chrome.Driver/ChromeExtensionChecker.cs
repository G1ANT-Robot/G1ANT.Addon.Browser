using G1ANT.Browser.Driver.Actions;
using G1ANT.Browser.Driver.Data;
using System;
using System.IO;

namespace G1ANT.Chrome.Driver
{
    public class ChromeExtensionChecker : IDisposable
    {
        private bool disposedValue;
        private readonly ChromeClient chromeClient;
        private string tabIdToClose;

        public ChromeExtensionChecker(ChromeClient chromeClient)
        {
            this.chromeClient = chromeClient;
            StartBrowserExtension();
        }

        protected void StartBrowserExtension()
        {
            var chromeServie = chromeClient.BrowserService as ChromeService;
            if (chromeServie == null || string.IsNullOrEmpty(chromeServie.StartingExtensionHtmlUrl))
                return;

            try
            {
                chromeServie.EventsService.OnTabUpdated += CloseReconnectingTab;
                var tmpPath = Path.Combine(Path.GetTempPath(), $"chrome-tmp-user-{Guid.NewGuid()}");
                var extraArgs = $"{ChromeArguments.Frame} {ChromeArguments.App}={chromeServie.StartingExtensionHtmlUrl}";
                chromeClient.OpenBrowserWithUrl("", extraArgs);
            }
            catch (TimeoutException ex)
            {
                if (!chromeClient.IsExtensionConnected())
                    throw ex;
            }
            catch
            {
                throw;
            }
            finally
            {
                chromeServie.EventsService.OnTabUpdated -= CloseReconnectingTab;
            }
        }

        protected void CloseReconnectingTab(BrowserTab tab)
        {
            var chromeServie = chromeClient.BrowserService as ChromeService;
            var uri = new Uri(chromeServie?.StartingExtensionHtmlUrl.ToLower());
            if (tab.Status == "complete" && tab.Url.ToLower() == uri.AbsoluteUri)
            {
                tabIdToClose = tab.Id;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (!string.IsNullOrEmpty(tabIdToClose))
                    {
                        chromeClient.CloseTab(new CloseTabAction()
                        {
                            TabId = tabIdToClose
                        });
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
