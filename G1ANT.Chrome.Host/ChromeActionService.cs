using G1ANT.Chromium.Host;
using G1ANT.Chromium.Host.Data;
using System.Windows.Forms;

namespace G1ANT.Chrome.Host
{
    public class ChromeActionService : ChromiumActionService
    {

        public ChromeActionService(ChromiumHost host) : base(host)
        {
        }

        protected override void ExitExtension()
        {
            host.ExitExtension();
        }
    }
}
