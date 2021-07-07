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
    [Command(Name = "browser.getouterhtml", Tooltip = "This command gets outer Html of a specified element")]

    public class BrowserGetOuterHtmlCommand : Command
    {
        public class Arguments : BrowserCommandArguments
        {
            [Argument(DefaultVariable = "timeoutbrowser", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(BrowserSettings.Timeout);

            [Argument(Tooltip = "Name of a variable where the outer html of a specified element will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");
        }

        public BrowserGetOuterHtmlCommand(AbstractScripter scripter) : base(scripter)
        {
        }

        public void Execute(Arguments arguments)
        {
            throw new NotImplementedException();
        }
    }
}
