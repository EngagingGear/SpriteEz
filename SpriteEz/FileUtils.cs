using System;
using System.IO;
using System.Reflection;

namespace SpriteEzNs
{
    public static class FileUtils
    {
        public static string GetDestinationPath(string outputDir, string file)
        {

            if (string.IsNullOrWhiteSpace(outputDir))
            {
                return file;
            }
            var dirName = Path.GetFullPath(outputDir);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            return Path.Combine(dirName, file);
        }
    }
}