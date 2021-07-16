using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace G1ANT.Chromium.Host
{
    internal class ChromiumResponseConfirmation
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public JObject Data { get; set; }

        public ChromiumResponseConfirmation(JObject data)
        {
            Data = data;
            Message = "confirmation";
        }

        public JObject GetJObject()
        {
            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(this));
        }
    }
}
