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

namespace G1ANT.Addon.Browser
{
    [Addon(Name = "Browser",
        Tooltip = "Commands to manipulate web browser content")]
    [Copyright(Author = "G1ANT LTD", Copyright = "G1ANT LTD", Email = "hi@g1ant.com", Website = "www.g1ant.com")]
    [License(Type = "LGPL", ResourceName = "License.txt")]
    [CommandGroup(Name = "browser", Tooltip = "Commands to work with web pages via supported web browsers.", IconName = "browsericon")]
    public class BrowserAddon : Language.Addon
    {
    }
}
