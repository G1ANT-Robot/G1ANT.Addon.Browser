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
    [Command(Name = "browser.setattribute", Tooltip = "This command sets a specified attribute of a specified element")]

    public class BrowserSetAttributeCommand : Command
    {
        public class Arguments : BrowserCommandArguments
        {
            [Argument(Required = true, Tooltip = "Name of an attribute or property to set its value")]
            public TextStructure Name { get; set; }

            [Argument(Tooltip = "Value to set")]
            public TextStructure Value { get; set; } = new TextStructure("");

            //[Argument(Tooltip = "Force setting attribute or property. 'ForceAttribute' to set html element attribute (like data-id), 'ForceProperty' to set DOM element property (like value, name or selectedIndex), 'PreferAttribute' or 'Default' to set atribute with fallback to property. Default is 'PreferAttribute'")]
            //public TextStructure Type { get; set; } = new TextStructure(AttributeOperationType.Default.ToString());

            [Argument(DefaultVariable = "timeoutbrowser", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(BrowserSettings.Timeout);
        }

        public BrowserSetAttributeCommand(AbstractScripter scripter) : base(scripter)
        { }


        public void Execute(Arguments arguments)
        {
            try
            {
                BrowserManager.CurrentWrapper.SetAttributeValue(
                    arguments,
                    arguments.Name.Value,
                    arguments.Value.Value,
                    arguments.Timeout.Value);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occured while setting '{arguments.Name.Value}' attribute. Search element phrase: '{arguments.Search.Value}', by: '{arguments.By.Value}'. Message: {ex.Message}", ex);
            }
        }
    }
}
