using G1ANT.Browser.Driver.Interfaces;
using G1ANT.Chrome.Driver;
using G1ANT.Language;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1ANT.Addon.Browser.Api
{
    public static class BrowserManager
    {
        private static BrowserWrapper currentWrapper = null;
        public static BrowserWrapper CurrentWrapper
        {
            get
            {
                if (currentWrapper == null)
                {
                    throw new InvalidOperationException("No browser instance attached. Please, use browser.open command first.");
                }
                return currentWrapper;
            }
            private set
            {
                currentWrapper = value;
            }
        }

        private static List<BrowserWrapper> wrappers = new List<BrowserWrapper>();

        private static string GetStartingExtensionHtmlPath()
        {
            var unpackFolder = AbstractSettingsContainer.Instance.UserDocsAddonFolder.FullName;
            var htmlFileName = "G1ANT.Addon.Browser.Starting.Extension.html";
            var containingAssemblyName = typeof(BrowserAddon).Assembly.GetName().Name;
            try
            {
                var fullResourceName = $"{containingAssemblyName}.{htmlFileName}";
                var fullPath = Path.Combine(unpackFolder, htmlFileName);
                Utils.SaveResourceToFile(fullPath, fullResourceName);
                return fullPath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private static IBrowserDriver GetBrowserDriver(string webBrowserName)
        {
            switch (webBrowserName.ToLower())
            {
                case "ie":
                case "iexplorer":
                case "internetexplorer":
                    break;
                case "chrome":
                    {
                        var driverService = ChromeService.CreateService();
                        driverService.StartingExtensionHtmlUrl = GetStartingExtensionHtmlPath();
                        return new ChromeClient(driverService);
                    }
                case "firefox":
                case "ff":
                    break;
                case "edge":
                    break;
            }
            throw new ArgumentException("Browser is not supported yet. It accepts one of following values: 'ie', 'chrome', 'firefox', 'edge'");
        }

        public static BrowserWrapper CreateWrapper(string webBrowserName, string url, TimeSpan timeout, bool noWait)
        {
            BrowserWrapper wrapper = new BrowserWrapper(GetBrowserDriver(webBrowserName));
            CurrentWrapper = wrapper;
            if (!string.IsNullOrEmpty(url))
            {
                CurrentWrapper.OpenUrl(url, timeout, noWait);
            }
            return CurrentWrapper;
        }

        public static void Switch(int id)
        {
            var selectedWrapper = wrappers.Where(x => x.Id == id).FirstOrDefault();
            CurrentWrapper = selectedWrapper ?? throw new InvalidOperationException($"Browser instance with id '{id}' does not exist");
        }

        public static void QuitCurrentWrapper()
        {
            if (CurrentWrapper != null)
            {
                wrappers.Remove(CurrentWrapper);
                CurrentWrapper = null;
            }
        }

        public static void RemoveWrapper(int id)
        {
            var toRemove = wrappers.Where(x => x.Id == id).FirstOrDefault();
            RemoveWrapper(toRemove);
        }

        public static void RemoveWrapper(BrowserWrapper wrapper)
        {
            wrappers.Remove(wrapper);
        }
    }
}
