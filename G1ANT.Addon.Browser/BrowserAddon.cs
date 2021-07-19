/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Browser
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using G1ANT.Language;
using System.Reflection;

namespace G1ANT.Addon.Browser
{
    [Addon(Name = "Browser",
        Tooltip = "Commands to manipulate web browser content")]
    [Copyright(Author = "G1ANT LTD", Copyright = "G1ANT LTD", Email = "hi@g1ant.com", Website = "www.g1ant.com")]
    [License(Type = "LGPL", ResourceName = "License.txt")]
    [CommandGroup(Name = "browser", Tooltip = "Commands to work with web pages via supported web browsers.", IconName = "browsericon")]
    public class BrowserAddon : Language.Addon
    {
        public override void LoadDlls()
        {
            UnpackDrivers();
            base.LoadDlls();
        }

        private void UnpackDrivers()
        {
            var unpackFolder = AbstractSettingsContainer.Instance.UserDocsAddonFolder.FullName;
            var embeddedResources = new List<string>()
            {
                { "G1ANT.Chrome.Host.exe" },
            };
            var containingAssemblyName = typeof(BrowserAddon).Assembly.GetName().Name;
            foreach (var embededResource in embeddedResources)
            {
                try
                {
                    var fullResourceName = $"{containingAssemblyName}.{embededResource}";
                    KillWorkingProcess(Path.GetFileNameWithoutExtension(embededResource));
                    using (FileStream stream = File.Create(Path.Combine(unpackFolder, embededResource)))
                    {
                        using (var io = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullResourceName))
                        {
                            using (BinaryReader binaryReader = new BinaryReader(io))
                            {
                                var data = binaryReader.ReadBytes((int)io.Length);
                                stream.Write(data, 0, data.Length);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private void KillWorkingProcess(string processName)
        {
            foreach (Process proc in Process.GetProcessesByName(processName))
            {
                try
                {
                    proc.Kill();
                }
                catch
                {

                }
            }
        }

    }
}
