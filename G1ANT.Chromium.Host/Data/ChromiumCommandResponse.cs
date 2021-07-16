using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1ANT.Chromium.Host.Data
{
    public class ChromiumCommandResponse
    {
        public const string MessageName = "commandResponse";

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }

        [JsonProperty("data")]
        public JObject Data { get; set; }
    }
}
