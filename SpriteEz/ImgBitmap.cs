using System;
using System.Drawing;
using System.IO;

// ReSharper disable once CheckNamespace
namespace SpriteEzNs
{
    /// <summary>
    /// This class represents the triplet of images. If necessary it
    /// generates highlighted and disabled images.
    /// </summary>
    class ImgBitmap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="files">The file names</param>
        /// <param name="opts">Options to determine generation plans
        /// </param>
        public ImgBitmap(ImgFile files, Config opts)
        {
            FileNames = files;
            var tmp = GetNewBitmap(files.NormalImgFilename);
            NormalImg = new Bitmap(tmp);

            if (!opts.GenerateWithFilters)
            {
                if (files.GrayImageFilename != null &&
                    files.GrayImageFilename != "auto" &&
                    files.GrayImageFilename != "none")
                {
                    DisabledImage = GetNewBitmap(files.GrayImageFilename);
                    if (DisabledImage.Height != NormalImg.Height || DisabledImage.Width != NormalImg.Width)
                        throw new ArgumentException(files.GrayImageFilename +
                                                    ": Gray images must be the same size as the normal image");
                }
                else if (opts.GenerateDisabled ||
                         files.GrayImageFilename == "auto" &&
                         files.GrayImageFilename != "none")
                {
                    DisabledImage = new Bitmap(NormalImg);
                    MakeGrayscale(DisabledImage, opts);
                }
                else
                    DisabledImage = null;

                if (files.HighlightImgFilename != null &&
                    files.HighlightImgFilename != "auto" &&
                    files.HighlightImgFilename != "none")
                {
                    HighlightImage = GetNewBitmap(files.HighlightImgFilename);
                    if (HighlightImage.Height != NormalImg.Height || HighlightImage.Width != NormalImg.Width)
                        throw new ArgumentException(files.HighlightImgFilename +
                                                    ": Highlight images must be the same size as the normal image");
                }

                else if (opts.GenerateHighlight ||
                         files.HighlightImgFilename == "auto" &&
                         files.HighlightImgFilename != "none")
                {
                    HighlightImage = new Bitmap(NormalImg);
                    Brighten(HighlightImage, opts);
                }
                else
                    HighlightImage = null;
            }

            if(!string.IsNullOrEmpty(opts.DerivedDirectory))
            {
                var normalFilename = Path.GetFileName(files.NormalImgFilename);
                var disabledFilename = Path.GetFileNameWithoutExtension(files.NormalImgFilename) + "_d" + Path.GetExtension(files.NormalImgFilename);
                var highlightFilename = Path.GetFileNameWithoutExtension(files.NormalImgFilename) + "_h" + Path.GetExtension(files.NormalImgFilename);
                NormalImg?.Save(Path.Combine(opts.DerivedDirectory, normalFilename));
                DisabledImage?.Save(Path.Combine(opts.DerivedDirectory, disabledFilename));
                HighlightImage?.Save(Path.Combine(opts.DerivedDirectory, highlightFilename));
            }
        }

        /// <summary>
        /// Reads a bitmap from a file, and, if it fails throws an exception with a good error message.
        /// </summary>
        /// <param name="filename">Image filename</param>
        /// <returns>Bitmap of that image</returns>
        /// <exception cref="ArgumentException">Thrown if the file cannot be opened.
        /// </exception>
        private Bitmap GetNewBitmap(string filename)
        {
            try
            {
                return new Bitmap(filename);
            }
            catch (Exception)
            {
                throw new ArgumentException("Could not open file " + filename);
            }
        }

        /// <summary>
        /// Convert the image to grayscale
        /// </summary>
        /// <param name="img">Image to convert</param>
        /// <param name="opts">Options to determine conversion strategy
        /// </param>
        void MakeGrayscale(Bitmap img, Config opts)
        {
            var height = img.Height;
            var width = img.Width;
            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    var color = img.GetPixel(x, y);
                    if (color.A != 0 && !IsGrayScaleColor(color, opts.GrayScalePixelThrehold))
                    {
                        color = RGBHSL.SetSaturation(color, 0);
                        color = RGBHSL.SetBrightness(color, color.GetBrightness() * opts.DisableMultiplier);
                        img.SetPixel(x, y, color);
                    }
                }
            }
        }

        /// <summary>
        /// Brighten the image.
        /// </summary>
        /// <param name="img">Image to brighten</param>
        /// <param name="opts">Options to determine the strategy</param>
        void Brighten(Bitmap img, Config opts)
        {
            var height = img.Height;
            var width = img.Width;
            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    var color = img.GetPixel(x, y);
                    if(color.A!=0 && !IsGrayScaleColor(color, opts.GrayScalePixelThrehold))
                        img.SetPixel(x, y, RGBHSL.SetBrightness(color, color.GetBrightness() * opts.HighlightMultiplier));
                }
            }
        }

        private bool IsGrayScaleColor(Color color, double threshold)
        {
            var r = color.R;
            var g = color.G;
            var b = color.B;
            var avg = (r + g + b) / 3d;
            return Math.Abs(r - avg) < threshold && Math.Abs(b - avg) < threshold && Math.Abs(g - avg) < threshold;
        }

        /// <summary>
        /// The file entry.
        /// </summary>
        public ImgFile FileNames;

        /// <summary>
        /// The normal image
        /// </summary>
        public Bitmap NormalImg;

        /// <summary>
        /// The gray image, or null if not required.
        /// </summary>
        public Bitmap DisabledImage;
        
        /// <summary>
        /// The brightened image, or null if not required.
        /// </summary>
        public Bitmap HighlightImage;
    }
}
