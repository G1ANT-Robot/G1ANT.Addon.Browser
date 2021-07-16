using G1ANT.Browser.Driver.Interfaces;
using G1ANT.Browser.Driver.Services;

namespace G1ANT.Chrome.Driver
{
    public class ChromeEventsServer : DriverServer<BrowserEventsService, IBrowserEvents>
    {
        protected override string ServerName => Configuration.EventsServerName;

        public ChromeEventsServer(BrowserEventsService commandService) : base(commandService)
        {
        }
    }
}
