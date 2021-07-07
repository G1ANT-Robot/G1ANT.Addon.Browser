/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Browser
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using G1ANT.Language;
using System;

namespace G1ANT.Addon.Browser
{
    [Command(Name = "browser.switch", Tooltip = "This command changes the currently active web browser instance")]
    public class BrowserSwitchCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "ID of the web browser instance")]
            public IntegerStructure Id { get; set; }
        }
       
        public BrowserSwitchCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        
        public void Execute(Arguments arguments)
        {
            throw new NotImplementedException();
        }
    }
}
