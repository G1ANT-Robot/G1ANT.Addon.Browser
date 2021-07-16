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
using G1ANT.Chrome.Driver;
using G1ANT.Language;
using System;
using System.Collections.Generic;

namespace G1ANT.Addon.Browser
{
    [Command(Name = "browser.open", Tooltip = "This command opens a new instance of a chosen web browser and optionally navigates to a specified URL address")]
    public class BrowserOpenCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Name of a web browser")]
            public TextStructure Type { get; set; }

            [Argument(Tooltip = "URL address of a webpage to be loaded")]
            public TextStructure Url { get; set; } = new TextStructure(string.Empty);

            [Argument(DefaultVariable = "timeoutbrowser", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(BrowserSettings.Timeout);

            [Argument(Tooltip = "By default, waits until the webpage fully loads")]
            public BooleanStructure NoWait { get; set; } = new BooleanStructure(false);

            [Argument(Tooltip = "Name of a variable where the command's result will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");
        }

        public BrowserOpenCommand(AbstractScripter scripter) : base(scripter)
        {
        }

        public void Execute(Arguments arguments)
        {
            OpenAction action = new OpenAction();
            action.Timeout = (int)arguments.Timeout.Value.TotalMilliseconds;
            action.Url = arguments.Url.Value;
            action.NoWait = arguments.NoWait.Value;

            ChromeClient client = new ChromeClient();
            var tab = client.Open(action);
        }
    }
}
