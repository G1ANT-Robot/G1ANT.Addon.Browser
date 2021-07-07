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
    [Command(Name = "browser.click", Tooltip = "This command clicks a specified element on an active webpage")]
    public class BrowserClickCommand : Command
    {
        public class Arguments : BrowserCommandArguments
        {
            [Argument(Tooltip = "If set to `true`, the command should wait for a new window to appear after clicking the specified element")]
            public BooleanStructure WaitForNewWindow { get; set; } = new BooleanStructure(false);

            [Argument(DefaultVariable = "timeoutbrowser", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public  override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(BrowserSettings.Timeout);
        }

        public BrowserClickCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        
        public void Execute(Arguments arguments)
        {
            throw new NotImplementedException();
        }
    }
}
