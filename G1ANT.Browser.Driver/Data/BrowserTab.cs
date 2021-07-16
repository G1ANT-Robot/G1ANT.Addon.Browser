
using System.Runtime.Serialization;

namespace G1ANT.Browser.Driver.Data
{
    [DataContract]
    public class BrowserTab
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}
