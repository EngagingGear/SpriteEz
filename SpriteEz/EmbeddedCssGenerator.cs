using System.Collections.Generic;
using System.Drawing;

// ReSharper disable once CheckNamespace
namespace SpriteEzNs
{
    public class EmbeddedCssGenerator : ICssGenerator
    {
        private const string EmbeddedCssTemplate =
            "background-image: url(data:image/png;charset=utf-8;base64,$$DATA$$); background-repeat: no-repeat; background-position: center; height: $$HEIGHT$$px; width: 100%;";

        public List<string> Generate(List<CssEntry> cssEntries, Config config)
        {
            var results = new List<string>();

            //For each css entry:
            foreach (var cssEntry in cssEntries)
            {
                //write combined class (normal + hover)
                var line = GenerateCssForEmbeddedImage(cssEntry.NormalImagePos, config.ImageClass,
                    config.NormalCssTemplate, cssEntry.ImgName, config.NormalSuffix, cssEntry.NormalImageCssText);
                results.Add(line);

                //write highlighted
                if (config.GenerateHighlight && !string.IsNullOrWhiteSpace(cssEntry.HighlightedImageCssText))
                {
                    line = GenerateCssForEmbeddedImage(cssEntry.HighlightImagePos, config.ImageClass,
                        config.HighlightCssTemplate, cssEntry.ImgName, config.HighlightSuffix,
                        cssEntry.HighlightedImageCssText);
                    results.Add(line);
                }

                //write disabled
                if (config.GenerateDisabled && !string.IsNullOrWhiteSpace(cssEntry.DisabledImageCssText))
                {
                    line = GenerateCssForEmbeddedImage(cssEntry.DisabledImagePos, config.ImageClass,
                        config.DisabledCssTemplate, cssEntry.ImgName, config.DisabledSuffix,
                        cssEntry.DisabledImageCssText);
                    results.Add(line);
                }
            }

            return results;
        }

        private string GenerateCssForEmbeddedImage(Rectangle rect, string imageClass, string template, string imageName,
            string suffix, string encodeImage)
        {
            var css = EmbeddedCssTemplate
                .Replace("$$HEIGHT$$", rect.Height.ToString())
                .Replace("$$DATA$$", encodeImage);


            var line = template
                .Replace("$$IMAGE-CLASS$$", imageClass)
                .Replace("$$NAME$$", imageName)
                .Replace("$$SUFFIX$$", suffix)
                .Replace("$$CSS$$", css);
            return line;
        }
    }
}