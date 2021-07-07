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
    [Command(Name = "browser.waitforvalue", Tooltip = "This command waits for a Javascript code to return a specified value")]
    public class BrowserWaitForValueCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "The full Javascript code to be evaluated in the browser")]
            public TextStructure Script { get; set; }

            [Argument(Required = true, Tooltip = "Expected value that will be returned by the script")]
            public TextStructure ExpectedValue { get; set; }

            [Argument(DefaultVariable = "timeoutie", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public  override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(BrowserSettings.Timeout);
        }

        public BrowserWaitForValueCommand(AbstractScripter scripter) : base(scripter)
        {
        }

        public void Execute(Arguments arguments)
        {
            throw new NotImplementedException();
        }
    }
}
