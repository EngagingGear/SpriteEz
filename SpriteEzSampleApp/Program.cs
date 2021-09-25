using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SpriteEzSampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var execDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var toolPath = Path.Combine(execDir, "SpriteEzTool", "SpriteEz.exe");

            var process = new Process
            {
                StartInfo =
                {
                    FileName = toolPath,
                    Arguments = $"-config ./config.json ./assets/*.png",
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
            try
            {
                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                var exitCode = process.ExitCode;
                Console.WriteLine($"ExitCode: {exitCode}, output: {output}");
                process.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception thrown: {exception.Message}");
            }
        }
    }
}
