using System.Collections.Generic;
using System.Drawing;

// ReSharper disable once CheckNamespace
namespace SpriteEz
{
    public class SpriteCssGenerator : ISpriteCssGenerator
    {
        public List<string> Generate(List<CssEntry> cssEntries, Config config, string spriteFileName = null)
        {
            var results = new List<string>();

            //First write image class
            results.Add(AddImageClass(config, spriteFileName));

            //For each css entry:
            foreach (var cssEntry in cssEntries)
            {
                //write combined class (normal + hover)
                var line = GenerateCssForImage(cssEntry.NormalImagePos, config.NormalCssTemplate, config.ImageClass,
                    cssEntry.ImgName, config.NormalSuffix);
                results.Add(line);


                //write highlighted
                if (config.GenerateHighlight)
                {
                    if (config.GenerateWithFilters)
                    {
                        line = GenerateHighlightedClass(cssEntry.NormalImagePos, config.HighlightCssTemplate,
                            config.ImageClass, cssEntry.ImgName, config.HighlightMultiplier, config.HighlightSuffix);
                    }
                    else
                    {
                        line = GenerateCssForImage(cssEntry.HighlightImagePos, config.HighlightCssTemplate,
                            config.ImageClass, cssEntry.ImgName, config.HighlightSuffix);
                    }

                    results.Add(line);
                }

                //write disabled
                if (config.GenerateDisabled)
                {
                    if (config.GenerateWithFilters)
                    {
                        line = GenerateDisabledClass(cssEntry.NormalImagePos, config.DisabledCssTemplate,
                            config.ImageClass, cssEntry.ImgName, config.DisableMultiplier, config.DisabledSuffix);
                    }
                    else
                    {
                        line = GenerateCssForImage(cssEntry.DisabledImagePos, config.DisabledCssTemplate,
                            config.ImageClass, cssEntry.ImgName, config.DisabledSuffix);
                    }

                    results.Add(line);
                }
            }

            return results;
        }

        private string AddImageClass(Config config, string spriteFileName)
        {
            var line = config.ImageTemplate
                .Replace("$$IMAGE-CLASS$$", config.ImageClass)
                .Replace("$$FILE$$", spriteFileName);
            return line;
        }

        private string GenerateCssForImage(Rectangle rectangle, string template, string imageClass, string imageName,
            string suffix)
        {
            var css =
                $"background-position: -{rectangle.X}px -{rectangle.Y}px; width: {rectangle.Width}px; height: {rectangle.Height}px";
            var line = template
                .Replace("$$IMAGE-CLASS$$", imageClass)
                .Replace("$$NAME$$", imageName)
                .Replace("$$SUFFIX$$", suffix)
                .Replace("$$CSS$$", css);
            return line;
        }

        private string GenerateHighlightedClass(Rectangle rectangle, string template, string imageClass,
            string imageName, double brightnessFactor, string suffix)
        {
            var css =
                $"background-position: -{rectangle.X}px -{rectangle.Y}px; width: {rectangle.Width}px; height: {rectangle.Height}px; filter: brightness({brightnessFactor});";
            var line = template
                .Replace("$$IMAGE-CLASS$$", imageClass)
                .Replace("$$NAME$$", imageName)
                .Replace("SUFFIX", suffix)
                .Replace("$$CSS$$", css);

            return line;
        }

        private string GenerateDisabledClass(Rectangle rectangle, string template, string imageClass, string imageName,
            double grayscaleFactor, string suffix)
        {
            var css =
                $"background-position: -{rectangle.X}px -{rectangle.Y}px; width: {rectangle.Width}px; height: {rectangle.Height}px; filter: grayscale({grayscaleFactor});";
            var line = template
                .Replace("$$IMAGE-CLASS$$", imageClass)
                .Replace("$$NAME$$", imageName)
                .Replace("SUFFIX", suffix)
                .Replace("$$CSS$$", css);

            return line;
        }
    }
}