﻿using G1ANT.Browser.Driver.Actions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Runtime.Serialization;

namespace G1ANT.Browser.Driver.Data
{
    [DataContract]
    [KnownType(typeof(SearchElementBase))]
    [KnownType(typeof(ActivateTabAction))]
    [KnownType(typeof(ClickAction))]
    [KnownType(typeof(CloseTabAction))]
    [KnownType(typeof(GetActiveTabAction))]
    [KnownType(typeof(GetAttributeAction))]
    [KnownType(typeof(GetHtmlAction))]
    [KnownType(typeof(GetOuterHtmlAction))]
    [KnownType(typeof(GetTableAction))]
    [KnownType(typeof(GetTextAction))]
    [KnownType(typeof(NewTabAction))]
    [KnownType(typeof(PressKeyAction))]
    [KnownType(typeof(RefreshAction))]
    [KnownType(typeof(SetAttributeAction))]
    [KnownType(typeof(SetUrlAction))]
    [KnownType(typeof(TypeAction))]
    public class ActionBase
    {
        protected const string NameSuffix = "Action";

        [DataMember]
        public int Timeout = 6000;

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
            return JsonConvert.SerializeObject(this, JsonSerializerSettings);
        }

        public JObject ToJObject()
        {
            return JObject.FromObject(this, JsonSerializer.Create(JsonSerializerSettings));
        }
    }
}
