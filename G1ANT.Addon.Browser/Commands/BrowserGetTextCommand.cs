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
    [Command(Name = "browser.gettext", Tooltip = "This command gets text (a value) of a specified element")]

    public class BrowserGetTextCommand : Command
    {
        public class Arguments : BrowserCommandArguments
        {
            [Argument(DefaultVariable = "timeoutbrowser", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(BrowserSettings.Timeout);

            [Argument(Tooltip = "Name of a variable where the value of a specified attribute will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");
        }

        public BrowserGetTextCommand(AbstractScripter scripter) : base(scripter)
        {
        }

        public void Execute(Arguments arguments)
        {
            try
            {
                var attributeValue = BrowserManager.CurrentWrapper.GetTextValue(
                    arguments,
                    arguments.Timeout.Value);

                Scripter.Variables.SetVariableValue(arguments.Result.Value, new TextStructure(attributeValue));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Search element phrase: '{arguments.Search.Value}', by: '{arguments.By.Value}'. Message: {ex.Message}", ex);
            }
        }
    }
}
