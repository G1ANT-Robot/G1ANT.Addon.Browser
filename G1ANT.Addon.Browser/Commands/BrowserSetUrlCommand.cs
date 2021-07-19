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
    [Command(Name = "browser.seturl", Tooltip = "This command navigates the currently active browser instance or tab to a specified URL address")]
    public class BrowserSetUrlCommand : Command
    {
        public class Arguments : CommandArguments
        {

            [Argument(Required = true, Tooltip = "Webpage address to be loaded")]
            public TextStructure Url { get; set; } = new TextStructure(string.Empty);

            [Argument(DefaultVariable = "timeoutbrowser", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public  override TimeSpanStructure Timeout { get; set; } = new TimeSpanStructure(BrowserSettings.Timeout);

            [Argument(Tooltip = "By default, waits until the webpage fully loads")]
            public BooleanStructure NoWait { get; set; } = new BooleanStructure(false);
            
        }
       
        public BrowserSetUrlCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        
        public void Execute(Arguments arguments)
        {
            try
            {
                BrowserManager.CurrentWrapper.SetUrl(
                    arguments.Url.Value, 
                    arguments.Timeout.Value, 
                    arguments.NoWait.Value);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occured while setting url on currently active browser instance. Url address: '{arguments.Url.Value}'. Message: {ex.Message}", ex);
            }
        }
    }
}
