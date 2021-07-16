using Microsoft.Win32;

namespace G1ANT.Chromium.Host
{
    public class ChromiumBrowserRegistrar
    {
        private readonly string regHostnameKeyLocation;

        public static ChromiumBrowserRegistrar GoogleChrome => new ChromiumBrowserRegistrar("Google Chrome", "SOFTWARE\\Google\\Chrome\\");
        public static ChromiumBrowserRegistrar MicrosoftEdge => new ChromiumBrowserRegistrar("Microsoft Edge", "SOFTWARE\\Microsoft\\Edge\\");

        public string BrowserName
        {
            get;
            private set;
        }

        protected ChromiumBrowserRegistrar(string browserName, string RegKeyBaseLocation)
        {
            BrowserName = browserName;
            regHostnameKeyLocation = RegKeyBaseLocation + "\\NativeMessagingHosts\\";
        }

        public bool IsRegistered(string Hostname, string ManifestPath)
        {
            string targetKeyPath = regHostnameKeyLocation + Hostname;

            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(targetKeyPath, true);

            if (regKey != null && regKey.GetValue("").ToString() == ManifestPath)
                return true;

            return false;
        }

        public void Register(string Hostname, string ManifestPath)
        {
            string targetKeyPath = regHostnameKeyLocation + Hostname;

            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(targetKeyPath, true);

            if (regKey == null)
                regKey = Registry.CurrentUser.CreateSubKey(targetKeyPath);

            regKey.SetValue("", ManifestPath, RegistryValueKind.String);

            regKey.Close();
        }

        public void Unregister(string Hostname)
        {
            string targetKeyPath = regHostnameKeyLocation + Hostname;

            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(targetKeyPath, true);
            if (regKey != null)
                regKey.DeleteSubKey("", true);
            regKey?.Close();
        }

        public override string ToString()
        {
            return BrowserName;
        }
    }
}
