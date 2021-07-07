using G1ANT.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1ANT.Addon.Browser.Api
{
    public class BrowserCommandArguments : BrowserIFrameArguments
    {
        [Argument(Required = true, Tooltip = "Phrase to find an element by")]
        public TextStructure Search { get; set; }

        [Argument(Tooltip = "Specifies an element selector: 'id', 'class', 'cssselector', 'tag', 'xpath', 'name', 'query', 'jquery'")]
        public TextStructure By { get; set; } = new TextStructure("id");
    }
}
