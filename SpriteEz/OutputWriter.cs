using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using SpriteEz.Utils;

namespace SpriteEz
{
    internal class OutputWriter : IOutputWriter
    {
        private readonly ICssGenerator _cssGenerator;
        private readonly IHtmlHelpFileGenerator _htmlHelpFileGenerator;
        private readonly Logger _logger;

        public OutputWriter(Logger logger, bool embedded)
        {
            _logger = logger;
            _cssGenerator = embedded ? new EmbeddedCssGenerator() : new SpriteCssGenerator();
            _htmlHelpFileGenerator = embedded
                ? new EmbeddedImageHtmlHelpFileGenerator(logger)
                : new SpriteHtmlHelpFileGenerator(logger);
        }

        public void WriteResults(List<ProcessedImageData> results, Config config)
        {
            var isScaled = false;
            var mediaBreakpointConstraints = new List<MediaBreakpointConstraint>();


            foreach (var result in results)
            {
                isScaled |= result.IsScaled;
                if (result.IsScaled)
                {
                    var constraint = result.ImageDescriptor.ScaleDescriptor.MediaBreakpointConstraint;
                    constraint.CssFileName = result.CssFileName;
                    mediaBreakpointConstraints.Add(constraint);
                }

                var cssEntries = result.CssEntries;

                var cssPath = FileUtils.GetDestinationPath(config.OutputDirectory, result.CssFileName);

                if (result.SpriteBitmap != null)
                {
                    var spritePath = FileUtils.GetDestinationPath(config.OutputDirectory, result.SpriteFileName);
                    var imageToSave = result.SpriteBitmap;
                    imageToSave.Save(spritePath, ImageFormat.Png);
                }

                // Create and save the CSS file using the templates from configuration file
                var cssLines = _cssGenerator.Generate(cssEntries, config, result.SpriteFileName);
                using var writer = new StreamWriter(cssPath);
                foreach (var cssLine in cssLines)
                {
                    writer.WriteLine(cssLine);
                }
            }

            var first = results.First();
            var imageNames = first.CssEntries.Select(cssEntry => cssEntry.ImgName).ToList();

            if (isScaled)
            {
                var adjustedConstraints = AdjustMediaConstraints(mediaBreakpointConstraints);

                var generalCssPath = FileUtils.GetDestinationPath(config.OutputDirectory, config.SpriteCssFile);
                using var writer = new StreamWriter(generalCssPath);
                foreach (var constraint in adjustedConstraints)
                {
                    writer.WriteLine(constraint);
                }
            }

            //generate and save html help file
            if (config.GenerateHelpFile
                && !string.IsNullOrWhiteSpace(config.HelpFile)
                && !string.IsNullOrWhiteSpace(config.SpriteCssFile))
            {
                _htmlHelpFileGenerator.GenerateFile(imageNames, config, config.SpriteCssFile);
            }
        }

        protected virtual List<MediaBreakpointConstraint> AdjustMediaConstraints(
            List<MediaBreakpointConstraint> constraints)
        {
            var list = constraints.OrderBy(x => x.MinWidth).ToList();

            MediaBreakpointConstraint prev = null;
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (prev != null)
                {
                    list[i].MaxWidth = prev.MinWidth - 1;
                }

                if (i == 0)
                {
                    list[i].MinWidth = 0;
                }

                prev = list[i];
            }

            return list.OrderBy(x => x.MinWidth).ToList();
        }
    }
}