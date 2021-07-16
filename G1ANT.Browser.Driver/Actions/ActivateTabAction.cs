using G1ANT.Browser.Driver.Data;
using System.Runtime.Serialization;

namespace G1ANT.Browser.Driver.Actions
{
    [DataContract]
    public class ActivateTabAction : ActionBase
    {
        [DataMember(IsRequired = true)]
        public string Search = "";

        [DataMember(IsRequired = true)]
        public string By = "";
    }
}
