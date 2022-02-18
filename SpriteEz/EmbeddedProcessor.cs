using System.Collections.Generic;
using System.Drawing;
using SpriteEz.Utils;

namespace SpriteEz
{
    internal class EmbeddedProcessor : ImageProcessorBase, IEmbeddedProcessor
    {
        public EmbeddedProcessor(Logger logger) : base(logger)
        {
        }

        public override ProcessedImageData Process(ImageDescriptor imageDescriptor, Config config)
        {
            var images = imageDescriptor.Bitmaps;

            var result = new List<CssEntry>();
            foreach (var image in images)
            {
                var cssEntry = CreateCssEntry(image.FileNames.ImgName);
                if (config.Compress)
                {
                    Logger.Log("Compressing embedded image");
                }

                var normalImage = config.Compress ? image.NormalImg.Compress() : image.NormalImg;
                cssEntry.NormalImageCssText = normalImage.Encode();
                cssEntry.NormalImagePos = new Rectangle(0, 0, normalImage.Width, normalImage.Height);

                if (image.DisabledImage != null)
                {
                    var disabledImage
                        = config.Compress ? image.DisabledImage.Compress() : image.DisabledImage;
                    cssEntry.DisabledImagePos = new Rectangle(0, 0, disabledImage.Width, disabledImage.Height);
                    cssEntry.DisabledImageCssText = disabledImage.Encode();
                }

                if (image.HighlightImage != null)
                {
                    cssEntry.HighlightImagePos = new Rectangle(0, 0, image.NormalImg.Width, image.NormalImg.Height);
                    var highlightImage
                        = config.Compress ? image.HighlightImage.Compress() : image.HighlightImage;
                    cssEntry.DisabledImagePos = new Rectangle(0, 0, highlightImage.Width, highlightImage.Height);
                    cssEntry.HighlightedImageCssText = highlightImage.Encode();
                }

                result.Add(cssEntry);
            }

            var cssFileName = GetFileName(config.SpriteCssFile, imageDescriptor?.ScaleDescriptor);
            return new ProcessedImageData(null, result, imageDescriptor, cssFileName);
        }
    }
}