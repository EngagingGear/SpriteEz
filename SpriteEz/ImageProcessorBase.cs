using System.IO;

namespace SpriteEz
{
    internal abstract class ImageProcessorBase : ISpriteDescriptorProcessor
    {
        // private static readonly int alphaTransparency = 10;
        // private static readonly int alphaFader = 70;
        protected readonly Logger Logger;

        protected ImageProcessorBase(Logger logger)
        {
            Logger = logger;
        }

        public abstract ProcessedImageData Process(ImageDescriptor imageSet, Config config);

        protected CssEntry CreateCssEntry(string imgName)
        {
            return new CssEntry(imgName);
        }

        protected string GetFileName(string cssFileName, ScaleDescriptor scaleDescriptor)
        {
            if (scaleDescriptor == null)
            {
                return cssFileName;
            }

            var fileName = Path.GetFileNameWithoutExtension(cssFileName);
            var fileExtension = Path.GetExtension(cssFileName);
            var suffix = scaleDescriptor.Key;
            return $"{fileName}-{suffix}{fileExtension}";
        }

        // protected Image CompressImage(Bitmap image)
        // {
        //     var compressor = new WuQuantizer();
        //     var compressedImage = compressor.QuantizeImage(image, alphaTransparency, alphaFader);
        //     return compressedImage;
        // }
    }
}