using System.IO;

namespace SpriteEz.Utils
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