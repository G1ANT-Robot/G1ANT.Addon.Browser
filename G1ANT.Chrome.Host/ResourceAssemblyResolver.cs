using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace G1ANT.Chrome.Host
{
    static public class ResourceAssemblyResolver
    {
        static private Dictionary<string, Assembly> _loadedAssemblies = new Dictionary<string, Assembly>();

        static public Assembly Resolve(object sender, ResolveEventArgs e)
        {
            return Resolve(e.Name);
        }

        static public Assembly Resolve(string resourceName)
        {
            if (resourceName.Contains(".resources"))
                return null;

            var assemblyName = new AssemblyName(resourceName);
            resourceName = assemblyName.Name;
            var resourceFullName = GetResourceFullName(typeof(ResourceAssemblyResolver).Assembly, resourceName);

            Assembly assembly = null;
            lock (_loadedAssemblies)
            {
                if (!_loadedAssemblies.TryGetValue(resourceFullName, out assembly))
                {
                    try
                    {
                        using (var io = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFullName))
                        {
                            using (BinaryReader binaryReader = new BinaryReader(io))
                            {
                                assembly = Assembly.Load(binaryReader.ReadBytes((int)io.Length));
                                _loadedAssemblies.Add(resourceFullName, assembly);
                            }
                        }
                    }
                    catch
                    { }
                }
            }
            return assembly;
        }

        static private string GetResourceFullName(Assembly containingAssembly, string resourceName)
        {
            var containingAssemblyName = containingAssembly.GetName().Name;
            var fullResourceName = $"{containingAssemblyName}.{resourceName}";
            var resourceNames = containingAssembly.GetManifestResourceNames();

            return FindResourceByFullNameWithoutExtension(fullResourceName, resourceNames)
                ?? FindResourceByPartialMatchWithoutExtension(resourceName, resourceNames)
                ?? throw new Exception($"Resource {fullResourceName} not found in {containingAssemblyName}");
        }

        private static string FindResourceByPartialMatchWithoutExtension(string resourceName, string[] resourceNames)
        {
            if (!resourceName.EndsWith(".dll") && !resourceName.EndsWith(".exe"))
                return FindResourceByPartialMatch($"{resourceName}.dll", resourceNames)
                    ?? FindResourceByPartialMatch($"{resourceName}.exe", resourceNames);

            return FindResourceByPartialMatch(resourceName, resourceNames);
        }

        private static string FindResourceByFullNameWithoutExtension(string fullResourceName, string[] resourceNames)
        {
            if (!fullResourceName.EndsWith(".dll") && !fullResourceName.EndsWith(".exe"))
                return FindResourceByFullNameExactMatch($"{fullResourceName}.dll", resourceNames)
                    ?? FindResourceByFullNameExactMatch($"{fullResourceName}.exe", resourceNames);

            return FindResourceByFullNameExactMatch(fullResourceName, resourceNames);
        }

        private static string FindResourceByPartialMatch(string resourceName, string[] resourceNames)
        {
            return resourceNames.FirstOrDefault(rn => rn.EndsWith($".{resourceName}", StringComparison.CurrentCultureIgnoreCase));
        }

        private static string FindResourceByFullNameExactMatch(string fullResourceName, string[] resourceNames)
        {
            return resourceNames.FirstOrDefault(rn => rn.Equals(fullResourceName, StringComparison.CurrentCultureIgnoreCase));
        }

    }
}
