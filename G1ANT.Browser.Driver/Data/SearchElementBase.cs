using System.Runtime.Serialization;

namespace G1ANT.Browser.Driver.Data
{
    [DataContract]
    abstract public class SearchElementBase : ActionBase
    {
        [DataMember]
        public string Search = "";

        [DataMember]
        public string By = ""; // Specifies an element selector: 'id', 'class', 'cssselector', 'tag', 'xpath', 'name', 'query', 'jquery'
    }
}
