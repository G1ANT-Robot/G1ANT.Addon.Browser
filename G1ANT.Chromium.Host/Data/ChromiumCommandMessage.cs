using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;

namespace G1ANT.Chromium.Host.Data
{
    public class ChromiumCommandMessage
    {
        public readonly string Id = Guid.NewGuid().ToString("N");
        public string Command;
        public JObject Args;

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
