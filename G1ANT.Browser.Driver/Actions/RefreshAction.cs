﻿using G1ANT.Browser.Driver.Data;
using System.Runtime.Serialization;

namespace G1ANT.Browser.Driver.Actions
{
    [DataContract]
    public class RefreshAction : ActionBase
    {
        [DataMember]
        public bool BypassCache = false;
    }
}
