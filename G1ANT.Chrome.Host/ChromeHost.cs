using G1ANT.Chrome.Driver;
using G1ANT.Chromium.Host;
using System;
using System.Windows.Forms;

namespace G1ANT.Chrome.Host
{
    public class ChromeHost : ChromiumHost
    {
        public override string Hostname => "com.g1ant.chromium.messaging";

        public override TimeSpan DefaultTimeout { get => ChromeService.Service.DefaultTimeout; }

        public ChromeHost() : base(false)
        {
            SupportedBrowsers.Add(ChromiumBrowserRegistrar.GoogleChrome);
        }

        public override void ExitExtension()
        {
            Application.Exit();
        }
    }
}
