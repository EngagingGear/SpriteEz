using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using nQuant;

namespace SpriteEz.Utils
{
    internal static class ImageExtensions
    {
        private static readonly int alphaTransparency = 10;
        private static readonly int alphaFader = 70;

        /// <summary>
        ///     Convert the image to grayscale
        /// </summary>
        /// <param name="img">Image to convert</param>
        /// <param name="opts">
        ///     Options to determine conversion strategy
        /// </param>
        public static void MakeGrayscale(this Bitmap img, Config opts)
        {
            var height = img.Height;
            var width = img.Width;
            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    var color = img.GetPixel(x, y);
                    if (color.A != 0 && !color.IsGrayScaleColor(opts.GrayScalePixelThrehold))
                    {
                        color = RGBHSL.SetSaturation(color, 0);
                        color = RGBHSL.SetBrightness(color, color.GetBrightness() * opts.DisableMultiplier);
                        img.SetPixel(x, y, color);
                    }
                }
            }
        }

        /// <summary>
        ///     Brighten the image.
        /// </summary>
        /// <param name="img">Image to brighten</param>
        /// <param name="opts">Options to determine the strategy</param>
        public static void Brighten(this Bitmap img, Config opts)
        {
            var height = img.Height;
            var width = img.Width;
            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    var color = img.GetPixel(x, y);
                    if (color.A != 0 && !color.IsGrayScaleColor(opts.GrayScalePixelThrehold))
                    {
                        img.SetPixel(x, y,
                            RGBHSL.SetBrightness(color, color.GetBrightness() * opts.HighlightMultiplier));
                    }
                }
            }
        }

        public static bool IsGrayScaleColor(this Color color, double threshold)
        {
            var r = color.R;
            var g = color.G;
            var b = color.B;
            var avg = (r + g + b) / 3d;
            return Math.Abs(r - avg) < threshold && Math.Abs(b - avg) < threshold && Math.Abs(g - avg) < threshold;
        }

        public static Bitmap Resize(this Bitmap srcImage, double scaleFactor)
        {
            var height = srcImage.Height;
            var width = srcImage.Width;

            var newHeight = (int)Math.Floor(height * scaleFactor);
            var newWidth = (int)Math.Floor(width * scaleFactor);

            var destRect = new Rectangle(0, 0, newWidth, newHeight);
            var destImage = new Bitmap(newWidth, newHeight);
            destImage.SetResolution(srcImage.HorizontalResolution, srcImage.VerticalResolution);

            using var graphics = Graphics.FromImage(destImage);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using var attr = new ImageAttributes();
            attr.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(srcImage, destRect, 0, 0, srcImage.Width, srcImage.Height, GraphicsUnit.Pixel, attr);
            return destImage;
        }

        public static Image Compress(this Bitmap image)
        {
            var compressor = new WuQuantizer();
            var compressedImage = compressor.QuantizeImage(image, alphaTransparency, alphaFader);
            return compressedImage;
        }

        public static string Encode(this Image image)
        {
            using var m = new MemoryStream();
            image.Save(m, ImageFormat.Png);
            var imageBytes = m.ToArray();

            return Convert.ToBase64String(imageBytes);
        }
    }
}