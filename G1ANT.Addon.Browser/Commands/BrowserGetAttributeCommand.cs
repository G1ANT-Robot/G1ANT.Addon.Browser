/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Browser
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using G1ANT.Addon.Browser.Api;
using G1ANT.Language;
using System;

namespace G1ANT.Addon.Browser
{
    [Command(Name = "browser.getattribute", Tooltip = "This command gets a specified attribute of a specified element")]

    public class BrowserGetAttributeCommand : Command
    {
        public class Arguments : BrowserCommandArguments
        {
            [Argument(Required = true, Tooltip = "Name of an attribute to obtain a value of")]
            public TextStructure Name { get; set; }

            [Argument(DefaultVariable = "timeoutbrowser", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public  override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(BrowserSettings.Timeout);

            [Argument(Tooltip = "Name of a variable where the value of a specified attribute will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");
        }
        
        public BrowserGetAttributeCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        
        public void Execute(Arguments arguments)
        {
            throw new NotImplementedException();
        }
    }
}
