using Newtonsoft.Json;

namespace G1ANT.Chromium.Host
{
    internal class ChromiumManifest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("path")]
        public string ExecuteablePath { get; set; }

        [JsonProperty("type")]
        public string Type { get { return "stdio"; } }

        [JsonProperty("allowed_origins")]
        public string[] AllowedOrigins { get; set; }

        public ChromiumManifest(string hostname, string description, string executeablePath, string[] allowedOrigins)
        {
            Name = hostname;
            Description = description;
            AllowedOrigins = allowedOrigins;
            ExecuteablePath = executeablePath;
        }
    }
}
