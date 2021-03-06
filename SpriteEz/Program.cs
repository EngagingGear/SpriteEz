using System;
using CmdLineEzNs;

namespace SpriteEz
{
    internal class Program
    {
        private static readonly Logger Logger = new();
        private static readonly ImageFileListGenerator FileListGenerator = new(Logger);
        private static Config _config;

        private static void Main(string[] args)
        {
            _config = new Config();
            var cmdLine = new CmdLineEz()
                .Config("config")
                .Flag("highlight", (_, v) => _config.GenerateHighlight = v)
                .Flag("disable", (_, v) => _config.GenerateDisabled = v)
                .Flag("generate-help", (_, v) => _config.GenerateHelpFile = v)
                .Flag("help", (_, v) => _config.GenerateHelpFile = v)
                .Param("hmultiplier", (_, v) => SetBMultiplier(_config, v))
                .Param("dmultiplier", (_, v) => SetGMultiplier(_config, v))
                .Param("gray-pixel-threshold", (_, v) => SetGThreshold(_config, v))
                .Param("img-file", (_, v) => _config.SpriteImgFile = v)
                .Param("css-file", (_, v) => _config.SpriteCssFile = v)
                .Param("ntemplate", (_, v) => _config.NormalCssTemplate = v)
                .Param("htemplate", (_, v) => _config.HighlightCssTemplate = v)
                .Param("dtemplate", (_, v) => _config.DisabledCssTemplate = v)
                .Param("help-file", (_, v) => _config.HelpFile = v)
                .Param("image-class", (_, v) => _config.ImageClass = v)
                .Param("imgtemplate", (_, v) => _config.ImageTemplate = v)
                .Flag("generate-with-filters", (_, v) => _config.GenerateWithFilters = v)
                .Param("out-dir", (_, v) => _config.OutputDirectory = v)
                .Param("normal-suffix", (_, v) => _config.NormalSuffix = v)
                .Param("highlighted-suffix", (_, v) => _config.HighlightSuffix = v)
                .Param("disabled-suffix", (_, v) => _config.DisabledSuffix = v)
                .Flag("compress", (_, v) => _config.Compress = v)
                .Flag("embedded", (_, v) => _config.Embedded = v)
                .Param("auto-size-xs", (_, v) => AsDouble(v, x => _config.AutoSizeXs = x))
                .Param("auto-size-sm", (_, v) => AsDouble(v, x => _config.AutoSizeSm = x))
                .Param("auto-size-md", (_, v) => AsDouble(v, x => _config.AutoSizeMd = x))
                .Param("auto-size-lg", (_, v) => AsDouble(v, x => _config.AutoSizeLg = x))
                .Param("auto-size-xl", (_, v) => AsDouble(v, x => _config.AutoSizeXl = x))
                .Param("auto-size-xxl", (_, v) => AsDouble(v, x => _config.AutoSizeXxl = x))
                .Flag("?")
                .AllowTrailing();
            cmdLine.Process(args);

            if (cmdLine.FlagVal("help") == true || cmdLine.FlagVal("?") == true)
            {
                ShowHelp();
            }
            else
            {
                var imgFiles = FileListGenerator.GenerateImageFileNames(cmdLine.TrailingVal(), _config);
                var generator = new SpriteEzGenerator(imgFiles, _config, Logger);
                generator.Generate();
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine(
                @"SpriteEz has the following command arguments
                highlight     -- Auto generate highlight images that are missing (used for highlighted images, such as mouseover)
                disabled      -- Generate gray images that are missing (used for disabled commands)
                hmultiplier   -- The amount to brighten auto generated highlight images (0.0 .. 0.99)
                dmultiplier   -- The amount to fade gray images (0.00 .. 0.99)
                img-file      -- Name of the sprite composite image file
                css-file      -- Name of file containing suitable css classes to use the image
                ntemplate     -- Template for css for normal images
                htemplate     -- Template for css for highlight images
                dtemplate     -- Template for css for disabled images
                compress      -- Compress output images
                embedded      -- Embed output images into css styles
                help          -- Display this help message
                ");
        }

        private static void SetGMultiplier(Config config, string value)
        {
            if (!double.TryParse(value, out config.DisableMultiplier) || config.DisableMultiplier <= 0 ||
                config.DisableMultiplier > 1)
            {
                throw new SpriteEzException($"Invalid gmultiplier: {value}, but be decimal value between 0 and 1");
            }
        }

        private static void SetBMultiplier(Config config, string value)
        {
            if (!double.TryParse(value, out config.HighlightMultiplier) || config.HighlightMultiplier <= 1 ||
                config.HighlightMultiplier > 2)
            {
                throw new SpriteEzException($"Invalid bmultiplier: {value}, but be decimal value between 1 and 2");
            }
        }

        private static void SetGThreshold(Config config, string value)
        {
            if (!double.TryParse(value, out config.GrayScalePixelThrehold) || config.GrayScalePixelThrehold < 0 ||
                config.GrayScalePixelThrehold > 1)
            {
                throw new SpriteEzException(
                    $"Invalid gray-scale pixel threshold: {value}, but be decimal value between 0 and 1");
            }
        }

        private static void AsDouble(string value, Action<double> setter, Func<double, bool> checker = null,
            string message = null)
        {
            if (!double.TryParse(value, out var doubleValue))
            {
                throw new SpriteEzException(message ?? $"Cannot convert {value} to double");
            }

            if (checker != null && !checker(doubleValue))
            {
                throw new SpriteEzException(message ?? $"Check failed for {doubleValue}");
            }

            setter(doubleValue);
        }
    }
}