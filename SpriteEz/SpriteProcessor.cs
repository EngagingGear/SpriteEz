using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SpriteEz
{
    internal class SpriteProcessor : ImageProcessorBase, ISpriteProcessor
    {
        public SpriteProcessor(Logger logger) : base(logger)
        {
        }

        public override ProcessedImageData Process(ImageDescriptor imageDescriptor, Config config)
        {
            var bitmaps = imageDescriptor.Bitmaps;
            // Find the dimensions, width is the sum of all image widths, height is 1-3 depending on
            // whether gray and highlight are included
            var width = bitmaps.Sum(item => item.NormalImg.Width);
            var height = bitmaps.Max(item => item.NormalImg.Height);
            var heightMultiplier = 3;
            var disabledRow = 1;
            var highlightRow = 2;
            // Note we insert a highlight or disabled row if the options indicate so or if there are any
            // highlight or disabled images supplied.
            if (config.GenerateWithFilters || !config.GenerateHighlight &&
                bitmaps.FirstOrDefault(item => item.HighlightImage != null) == null)
            {
                highlightRow = -1;
                heightMultiplier--;
            }

            if (config.GenerateWithFilters || !config.GenerateDisabled &&
                bitmaps.FirstOrDefault(item => item.DisabledImage != null) == null)
            {
                disabledRow = -1;
                highlightRow -= 1;
                heightMultiplier--;
            }

            //Create a big enough sprite to contain it.
            var sprite = new Bitmap(width, height * heightMultiplier);
            var graphics = Graphics.FromImage(sprite);

            var cssEntries = new List<CssEntry>();
            var x = 0;
            foreach (var image in bitmaps)
            {
                var cssEntry = CreateCssEntry(image.FileNames.ImgName);

                cssEntry.NormalImagePos = new Rectangle(x, 0, image.NormalImg.Width, image.NormalImg.Height);
                graphics.DrawImage(image.NormalImg, new Point(x, 0));

                if (image.DisabledImage != null)
                {
                    cssEntry.DisabledImagePos = new Rectangle(x, disabledRow * height, image.DisabledImage.Width,
                        image.DisabledImage.Height);
                    graphics.DrawImage(image.DisabledImage, new Point(x, disabledRow * height));
                }

                if (image.HighlightImage != null)
                {
                    cssEntry.HighlightImagePos = new Rectangle(x, highlightRow * height, image.HighlightImage.Width,
                        image.HighlightImage.Height);
                    graphics.DrawImage(image.HighlightImage, new Point(x, highlightRow * height));
                }

                x += image.NormalImg.Width;
                cssEntries.Add(cssEntry);
            }

            var cssFileName = GetFileName(config.SpriteCssFile, imageDescriptor?.ScaleDescriptor);
            var spriteFileName = GetFileName(config.SpriteImgFile, imageDescriptor?.ScaleDescriptor);

            var spriteData = new ProcessedImageData(sprite, cssEntries, imageDescriptor, cssFileName, spriteFileName);
            return spriteData;
        }
    }
}