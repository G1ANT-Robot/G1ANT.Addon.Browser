using G1ANT.Browser.Driver.Data;
using System.Runtime.Serialization;

namespace G1ANT.Browser.Driver.Actions
{
    [DataContract]
    public class NewTabAction : ActionBase
    {
        [DataMember(IsRequired = true)]
        public string Url = "";

        [DataMember]
        public bool NoWait = false;
    }
}
