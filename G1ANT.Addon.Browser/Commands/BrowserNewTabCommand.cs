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
using G1ANT.Browser.Driver.Actions;
using G1ANT.Browser.Driver.Data;
using G1ANT.Chrome.Driver;
using G1ANT.Language;
using System;

namespace G1ANT.Addon.Browser
{
    [Command(Name = "browser.newtab", Tooltip = "This command adds a new tab to the current browser")]
    public class BrowserNewTabCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Tooltip = "Webpage address to load")]
            public TextStructure Url { get; set; }

            [Argument(DefaultVariable = "timeoutbrowser", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public  override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(BrowserSettings.Timeout);

            [Argument(Tooltip = "By default, waits until the webpage fully loads")]
            public BooleanStructure NoWait { get; set; } = new BooleanStructure(false);
            
        }

        public BrowserNewTabCommand(AbstractScripter scripter) : base(scripter)
        {
        }

        public void Execute(Arguments arguments)
        {
            NewTabAction action = new NewTabAction();
            action.Timeout = (int)arguments.Timeout.Value.TotalMilliseconds;
            action.Url = arguments.Url.Value;
            action.NoWait = arguments.NoWait.Value;

            ChromeClient client = new ChromeClient();
            var tab = client.NewTab(action);
        }
    }
}
