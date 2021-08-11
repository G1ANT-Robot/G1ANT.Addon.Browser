using System.Runtime.Serialization;

namespace G1ANT.Browser.Driver.Data
{
    [DataContract]
    public class ActionResponse
    {
        [DataMember(IsRequired = true)]
        public bool Succeedded = false;

        [DataMember]
        public string JsonData;
    }
}
