/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Browser
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using G1ANT.Language;
using System;

namespace G1ANT.Addon.Browser
{
    [Command(Name = "browser.refresh", Tooltip = "This command refreshes the current tab content in the web browser")]
    public class BrowserRefreshCommand : Command
    {
        public class Arguments : CommandArguments
        {
        }

        public BrowserRefreshCommand(AbstractScripter scripter) : base(scripter)
        {
        }

        public void Execute(Arguments arguments)
        {
            throw new NotImplementedException();
        }
    }
}
