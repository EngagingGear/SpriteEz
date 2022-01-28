using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SpriteEz.Utils
{
    public static class ImageUtils
    {
        private static readonly List<string> SupportedExtensions = new() { "png", "gif" };

        public static bool IsSupportedImageExtension(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            var ext = Path.GetExtension(fileName).TrimStart('.');
            return SupportedExtensions.Contains(ext);
        }

        public static bool IsAnimatedImage(string fileName)
        {
            var image = Image.FromFile(fileName);
            //or just ImageAnimator.CanAnimate(image);
            var frameDimensions = new FrameDimension(image.FrameDimensionsList[0]);
            var framesCount = image.GetFrameCount(frameDimensions);
            image.Dispose();
            return framesCount > 1;
        }
    }
}