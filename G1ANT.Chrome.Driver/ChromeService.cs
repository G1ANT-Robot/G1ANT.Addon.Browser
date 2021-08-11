using G1ANT.Browser.Driver.Services;

namespace G1ANT.Chrome.Driver
{
    public class ChromeService : BrowserService
    {
        private static ChromeService _chromeService;
        public static ChromeService CreateService()
        {
            _chromeService = _chromeService ?? new ChromeService();
            return _chromeService;
        }

        private ChromeEventsServer eventsServer;

        public string StartingExtensionHtmlUrl { get; set; }

        private ChromeService()
        {
            EventsService = new BrowserEventsService();
            eventsServer = new ChromeEventsServer(EventsService);
            eventsServer.Start();
        }
    }
}
