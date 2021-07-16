using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace G1ANT.Chromium.Host.Data
{
    public class ChromiumMessage
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("data")]
        public JObject Data { get; set; }
    }
}
