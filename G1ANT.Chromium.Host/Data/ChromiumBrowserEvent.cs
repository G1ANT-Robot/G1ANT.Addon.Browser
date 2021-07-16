using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace G1ANT.Chromium.Host.Data
{
    public class ChromiumBrowserEvent
    {
        public const string MessageName = "browserEvent";

        [JsonProperty("Event")]
        public string Event { get; set; }

        [JsonProperty("data")]
        public JObject Data { get; set; }
    }
}
