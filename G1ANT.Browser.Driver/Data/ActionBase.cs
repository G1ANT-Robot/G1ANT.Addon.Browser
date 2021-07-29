using G1ANT.Browser.Driver.Actions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Runtime.Serialization;

namespace G1ANT.Browser.Driver.Data
{
    [DataContract]
    [KnownType(typeof(SearchCriteriaBase))]
    [KnownType(typeof(ActivateTabAction))]
    [KnownType(typeof(FindTabAction))]
    [KnownType(typeof(ClickAction))]
    [KnownType(typeof(CloseTabAction))]
    [KnownType(typeof(GetActiveTabAction))]
    [KnownType(typeof(GetAttributeAction))]
    [KnownType(typeof(GetHtmlAction))]
    [KnownType(typeof(GetInnerHtmlAction))]
    [KnownType(typeof(GetOuterHtmlAction))]
    [KnownType(typeof(GetTableAction))]
    [KnownType(typeof(GetTextAction))]
    [KnownType(typeof(NewTabAction))]
    [KnownType(typeof(OpenAction))]
    [KnownType(typeof(PressKeyAction))]
    [KnownType(typeof(RefreshAction))]
    [KnownType(typeof(SetAttributeAction))]
    [KnownType(typeof(SetUrlAction))]
    [KnownType(typeof(TypeTextAction))]
    public class ActionBase
    {
        protected const string NameSuffix = "Action";

        [DataMember]
        public TimeSpan Timeout = TimeSpan.FromSeconds(1);

        public string CommandName
        {
            get 
            {
                var name = GetType().Name;
                if (name.EndsWith(NameSuffix))
                    name = name.Substring(0, name.Length - NameSuffix.Length);
                return name; 
            }
        }

        protected JsonSerializerSettings JsonSerializerSettings => new JsonSerializerSettings()
        {
           ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public JObject ToJObject()
        {
            return JObject.FromObject(this);
        }
    }
}
