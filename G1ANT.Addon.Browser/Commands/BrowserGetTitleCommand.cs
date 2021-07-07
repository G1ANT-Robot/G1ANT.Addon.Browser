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
    [Command(Name = "browser.gettitle", Tooltip = "This command gets the title of the currently active web browser instance")]
    public class BrowserGetTitleCommand : Command
    {
        public class Arguments : CommandArguments
        {

            [Argument(Tooltip = "Name of a variable where the title will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");
        }

        public BrowserGetTitleCommand(AbstractScripter scripter) : base(scripter)
        {
        }

        public void Execute(Arguments arguments)
        {
            throw new NotImplementedException();
        }
    }
}
