using System;
using System.IO;
using System.Reflection;

namespace G1ANT.Chromium.Host
{
    internal static class Utils
    {
        static public string AssemblyLoadDirectory()
        {
            var codeBase = Assembly.GetEntryAssembly().CodeBase;
            var uri = new Uri(codeBase);
            var path = Uri.UnescapeDataString(uri.LocalPath);
            return Path.GetDirectoryName(path);
        }

        static public string AssemblyExecutablePath()
        {
            var codeBase = Assembly.GetEntryAssembly().CodeBase;
            var uri = new Uri(codeBase);
            return Uri.UnescapeDataString(uri.LocalPath);
        }
    }
}
