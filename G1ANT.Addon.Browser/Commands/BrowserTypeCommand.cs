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
    [Command(Name = "browser.type", Tooltip = "This command types text into a specified element")]
    public class BrowserTypeCommand : Command
    {
        public class Arguments : BrowserCommandArguments
        {
            [Argument(Required = true, Tooltip = "Text to be typed")]
            public TextStructure Text { get; set; }

            [Argument(DefaultVariable = "timeoutbrowser", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public  override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(BrowserSettings.Timeout);
        }

        public BrowserTypeCommand(AbstractScripter scripter) : base(scripter)
        {
        }

        public void Execute(Arguments arguments)
        {
            try
            {
                int timeout = (int)arguments.Timeout.Value.TotalMilliseconds;

                BrowserManager.CurrentWrapper.SetFocus(
                    arguments,
                    arguments.Timeout.Value);

                KeyboardTyper.TypeWithSendInput(arguments.Text.Value,
                    null,
                    null,
                    Scripter.LastWindow,
                    timeout,
                    true,
                    50,
                    Scripter);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occured while typing text to element. Text: '{arguments.Text.Value}'. 'Search element phrase: '{arguments.Search.Value}', by: '{arguments.By.Value}'. Message: {ex.Message}", ex);
            }
        }
    }
}
