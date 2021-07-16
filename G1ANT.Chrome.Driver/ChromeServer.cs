using G1ANT.Browser.Driver;
using G1ANT.Browser.Driver.Interfaces;
using G1ANT.Browser.Driver.Services;

namespace G1ANT.Chrome.Driver
{
    public class ChromeServer<ServiceT> : DriverServer<ServiceT, IBrowserAction>
    {
        protected override string ServerName => Configuration.ServerName;

        public ChromeServer(ServiceT commandService) : base(commandService)
        {
        }
    }
}
