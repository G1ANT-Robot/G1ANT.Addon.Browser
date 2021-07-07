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
    [Command(Name = "browser.activatetab", Tooltip = "This command activates a browser tab specified by a part of its title or URL address")]
    public class BrowserActivateTabCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Tab searching phrase")]
            public TextStructure Search { get; set; }

            [Argument(Required = true, Tooltip = "Tab searching constraint: `title` or `url`")]
            public TextStructure By { get; set; }
        }

        public BrowserActivateTabCommand(AbstractScripter scripter) : base(scripter)
        {
        }
        
        public void Execute(Arguments arguments)
        {
            throw new NotImplementedException();
        }
    }
}
