/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Browser
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using G1ANT.Addon.Browser.Api;
using G1ANT.Language;

namespace G1ANT.Addon.Browser
{
    [Command(Name = "browser.callfunction", Tooltip = "This command calls a function on a specified element")]
    public class BrowserCallFunctionCommand : Command
    {
        public class Arguments : BrowserCommandArguments
        {
            [Argument(Required = true, Tooltip = "Name of a function to call")]
            public TextStructure FunctionName { get; set; }

            [Argument(Tooltip = "Parameters to be passed to the function")]
            public ListStructure Parameters { get; set; }

            [Argument(Required = true, Tooltip = "Function call type: `javascript` or `jquery`")]
            public TextStructure Type { get; set; } = new TextStructure("javascript");

            [Argument(DefaultVariable = "timeoutbrowser", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public  override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(BrowserSettings.Timeout); 
        }

        public BrowserCallFunctionCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        
        public void Execute(Arguments arguments)
        {
            throw new NotImplementedException();
        }
    }
}
