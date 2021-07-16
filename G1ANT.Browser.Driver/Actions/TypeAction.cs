using G1ANT.Browser.Driver.Data;
using System.Runtime.Serialization;

namespace G1ANT.Browser.Driver.Actions
{
    [DataContract]
    public class TypeAction : SearchElementBase
    {
        [DataMember]
        public string Text = "";
    }
}
