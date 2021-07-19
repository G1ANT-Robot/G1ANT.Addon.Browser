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
    [Command(Name = "browser.geturl", Tooltip = "This command gets url of the current tab")]
    public class BrowserGetUrlCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Tooltip = "Name of a variable where the value of a specified attribute will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");

        }
        public BrowserGetUrlCommand(AbstractScripter scripter) : base(scripter)
        {
        }

        public void Execute(Arguments arguments)
        {
            try
            {
                var activeTab = BrowserManager.CurrentWrapper.GetActiveTab(
                    arguments.Timeout.Value);

                Scripter.Variables.SetVariableValue(arguments.Result.Value, new TextStructure(activeTab.Url));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occured while getting Url of currently active browser instance. Message: {ex.Message}", ex);
            }
        }
    }
}
