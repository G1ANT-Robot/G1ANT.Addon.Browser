using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1ANT.Browser.Driver.Services
{
    public class BrowserService
    {
        public BrowserEventsService EventsService { get; protected set; }

        public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(5);
    }
}
