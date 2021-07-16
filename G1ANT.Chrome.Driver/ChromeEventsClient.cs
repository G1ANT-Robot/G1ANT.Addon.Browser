using G1ANT.Browser.Driver.Services;

namespace G1ANT.Chrome.Driver
{
    public class ChromeEventsClient : BrowserEventsClient
    {
        protected override string ServerName => Configuration.EventsServerName;
    }
}
