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
using G1ANT.Addon.Browser.Api;

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
            UnpackFiles();
            UnpackBrowserHosts();
            base.LoadDlls();
        }

        private void UnpackFiles()
        {
        }

        private void UnpackBrowserHosts()
        {
            var unpackFolder = AbstractSettingsContainer.Instance.UserDocsAddonFolder.FullName;
            var embeddedHosts = new List<string>()
            {
                { "G1ANT.Chrome.Host.exe" },
            };
            var containingAssemblyName = typeof(BrowserAddon).Assembly.GetName().Name;
            foreach (var hostName in embeddedHosts)
            {
                try
                {
                    var fullResourceName = $"{containingAssemblyName}.{hostName}";
                    var hostPath = Path.Combine(unpackFolder, hostName);
                    if (IsUpdateNeeded(hostPath, fullResourceName))
                    {
                        KillWorkingProcess(Path.GetFileNameWithoutExtension(hostName));
                        Utils.SaveResourceToFile(hostPath, fullResourceName);
                        Process.Start(hostPath, "-register");
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private bool IsUpdateNeeded(string currentFilePath, string newFileResourceName)
        {
            if (!File.Exists(currentFilePath))
                return true;

            var currentVersion = new Version(FileVersionInfo.GetVersionInfo(currentFilePath).FileVersion);
            var newVersion = new Version(GetResourceVersion(newFileResourceName).FileVersion);
            return currentVersion < newVersion;
        }

        private FileVersionInfo GetResourceVersion(string resourceName)
        {
            var tmpFilename = Path.GetTempFileName();
            Utils.SaveResourceToFile(tmpFilename, resourceName);
            var version = FileVersionInfo.GetVersionInfo(tmpFilename);
            File.Delete(tmpFilename);
            return version;
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
                { }
            }
        }

    }
}
