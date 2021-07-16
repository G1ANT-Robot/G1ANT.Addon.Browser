using System;
using System.IO;
using System.Reflection;

namespace G1ANT.Chromium.Host
{
    internal static class Utils
    {
        static public string AssemblyLoadDirectory()
        {
            string codeBase = Assembly.GetEntryAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        static public string AssemblyExecutablePath()
        {
            string codeBase = Assembly.GetEntryAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            return Uri.UnescapeDataString(uri.Path);
        }
    }
}
