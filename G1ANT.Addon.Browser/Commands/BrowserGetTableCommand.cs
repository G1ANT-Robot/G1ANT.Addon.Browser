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
    [Command(Name = "browser.gettable", Tooltip = "This command gets table from a website and assigns its content to a variable of datatable structure")]
    public class BrowserGetTableCommand : Command
    {
        public class Arguments : BrowserCommandArguments
        {
            [Argument(Tooltip = "Name of a datatable variable where the table content from a website will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");
        }

        public BrowserGetTableCommand(AbstractScripter scripter) : base(scripter)
        {
        }

        public void Execute(Arguments arguments)
        {
            throw new NotImplementedException();
        }
    }
}
