using System.IO;
using System.Reflection;

namespace G1ANT.Addon.Browser.Api
{
    public static class Utils
    {
        public static void SaveResourceToFile(string filePath, string resourceName)
        {
            using (FileStream stream = File.Create(filePath))
            {
                var data = GetResourceBinary(resourceName);
                stream.Write(data, 0, data.Length);
            }
        }

        public static byte[] GetResourceBinary(string resourceName)
        {
            using (var io = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (BinaryReader binaryReader = new BinaryReader(io))
                {
                    return binaryReader.ReadBytes((int)io.Length);
                }
            }
        }
    }
}
