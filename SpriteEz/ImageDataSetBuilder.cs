using System;
using System.Collections.Generic;
using System.Drawing;
using SpriteEz.Utils;

namespace SpriteEz
{
    internal class ImageDataSetBuilder : IImageDataSetBuilder
    {
        public ImageDataSet Build(List<ImgFile> imgFiles, Config config)
        {
            var items = new List<ImageDescriptor>();
            var isAutoScaled = false;

            if (config.AutoSizeXs > 0)
            {
                isAutoScaled = true;
                var scaleDescriptor = CreateScaleDescriptor(KnownAutoSizes.Xs, config.AutoSizeXs);
                items.Add(CreateImageDescriptor(imgFiles, config, scaleDescriptor));
            }

            if (config.AutoSizeSm > 0)
            {
                isAutoScaled = true;
                var scaleDescriptor = CreateScaleDescriptor(KnownAutoSizes.Sm, config.AutoSizeSm);
                items.Add(CreateImageDescriptor(imgFiles, config, scaleDescriptor));
            }

            if (config.AutoSizeMd > 0)
            {
                isAutoScaled = true;
                var scaleDescriptor = CreateScaleDescriptor(KnownAutoSizes.Md, config.AutoSizeMd);
                items.Add(CreateImageDescriptor(imgFiles, config, scaleDescriptor));
            }

            if (config.AutoSizeLg > 0)
            {
                isAutoScaled = true;
                var scaleDescriptor = CreateScaleDescriptor(KnownAutoSizes.Lg, config.AutoSizeLg);
                items.Add(CreateImageDescriptor(imgFiles, config, scaleDescriptor));
            }

            if (config.AutoSizeXl > 0)
            {
                isAutoScaled = true;
                var scaleDescriptor = CreateScaleDescriptor(KnownAutoSizes.Xl, config.AutoSizeXl);
                items.Add(CreateImageDescriptor(imgFiles, config, scaleDescriptor));
            }

            if (config.AutoSizeXxl > 0)
            {
                isAutoScaled = true;
                var scaleDescriptor = CreateScaleDescriptor(KnownAutoSizes.Xxl, config.AutoSizeXxl);
                items.Add(CreateImageDescriptor(imgFiles, config, scaleDescriptor));
            }

            if (!isAutoScaled)
            {
                items.Add(CreateImageDescriptor(imgFiles, config));
            }

            return new ImageDataSet(items);
        }

        private ScaleDescriptor CreateScaleDescriptor(string key, double scaleFactor)
        {
            return new ScaleDescriptor(key, scaleFactor, KnownAutoSizes.GetByCode(key));
        }


        private ImageDescriptor CreateImageDescriptor(List<ImgFile> imgFiles, Config config,
            ScaleDescriptor scaleDescriptor = null)
        {
            var bitmaps = new List<ImgBitmap>();
            foreach (var imgFile in imgFiles)
            {
                bitmaps.Add(CreateImgBitmap(imgFile, config, scaleDescriptor?.ScaleFactor ?? 0.0d));
            }

            //     // 2. Create an array of ImgBitmap which contains bitmaps for the three forms.
            //var bitmaps = imgFiles.Select(item => new ImgBitmap(item, config)).ToArray();

            return new ImageDescriptor(bitmaps, scaleDescriptor);
        }

        private ImgBitmap CreateImgBitmap(ImgFile imgFile, Config config, double scaleFactor)
        {
            var tmp = GetNewBitmap(imgFile.NormalImgFilename, scaleFactor);
            var normalImg = new Bitmap(tmp);
            Bitmap disabledImage = null;
            Bitmap highlightedImage = null;

            if (!config.GenerateWithFilters)
            {
                disabledImage = ProcessDisabledImage(normalImg, imgFile, config, scaleFactor);
                highlightedImage = ProcessHighlightedImage(normalImg, imgFile, config, scaleFactor);
            }

            //todo DerivedDirectory configuration setting
            // if(!string.IsNullOrEmpty(opts.DerivedDirectory))
            // {
            //     var normalFilename = Path.GetFileName(files.NormalImgFilename);
            //     var disabledFilename = Path.GetFileNameWithoutExtension(files.NormalImgFilename) + "_d" + Path.GetExtension(files.NormalImgFilename);
            //     var highlightFilename = Path.GetFileNameWithoutExtension(files.NormalImgFilename) + "_h" + Path.GetExtension(files.NormalImgFilename);
            //     NormalImg?.Save(Path.Combine(opts.DerivedDirectory, normalFilename));
            //     DisabledImage?.Save(Path.Combine(opts.DerivedDirectory, disabledFilename));
            //     HighlightImage?.Save(Path.Combine(opts.DerivedDirectory, highlightFilename));
            // }


            return new ImgBitmap(imgFile, normalImg, disabledImage, highlightedImage);
        }

        private Bitmap ProcessDisabledImage(Bitmap normalImage, ImgFile imgFile, Config config, double scaleFactor)
        {
            Bitmap disabledImage = null;
            if (imgFile.GrayImageFilename != null &&
                imgFile.GrayImageFilename != "auto" &&
                imgFile.GrayImageFilename != "none")
            {
                disabledImage = GetNewBitmap(imgFile.GrayImageFilename, scaleFactor);
                if (disabledImage.Height != normalImage.Height || disabledImage.Width != normalImage.Width)
                {
                    throw new ArgumentException(imgFile.GrayImageFilename +
                                                ": Gray images must be the same size as the normal image");
                }
            }
            else if (config.GenerateDisabled ||
                     imgFile.GrayImageFilename == "auto" &&
                     imgFile.GrayImageFilename != "none")
            {
                disabledImage = CreateGrayscaleImage(normalImage, config);
            }

            return disabledImage;
        }

        private Bitmap ProcessHighlightedImage(Bitmap normalImage, ImgFile imgFile, Config config, double scaleFactor)
        {
            Bitmap highlightImage = null;

            if (imgFile.HighlightImgFilename != null &&
                imgFile.HighlightImgFilename != "auto" &&
                imgFile.HighlightImgFilename != "none")
            {
                highlightImage = GetNewBitmap(imgFile.HighlightImgFilename, scaleFactor);
                if (highlightImage.Height != normalImage.Height || highlightImage.Width != normalImage.Width)
                {
                    throw new ArgumentException(imgFile.HighlightImgFilename +
                                                ": Highlight images must be the same size as the normal image");
                }
            }

            else if (config.GenerateHighlight ||
                     imgFile.HighlightImgFilename == "auto" &&
                     imgFile.HighlightImgFilename != "none")
            {
                highlightImage = CreateHighlightedImage(normalImage, config);
            }

            return highlightImage;
        }

        /// <summary>
        ///     Reads a bitmap from a file, and, if it fails throws an exception with a good error message.
        /// </summary>
        /// <param name="filename">Image filename</param>
        /// <param name="scaleFactor"></param>
        /// <returns>Bitmap of that image</returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if the file cannot be opened.
        /// </exception>
        private Bitmap GetNewBitmap(string filename, double scaleFactor)
        {
            try
            {
                var bitmap = new Bitmap(filename);
                return scaleFactor > 0 ? bitmap.Resize(scaleFactor) : bitmap;
            }
            catch (Exception)
            {
                throw new ArgumentException("Could not open file " + filename);
            }
        }


        private Bitmap CreateGrayscaleImage(Bitmap baseImage, Config opts)
        {
            var resultImage = new Bitmap(baseImage);
            resultImage.MakeGrayscale(opts);
            return resultImage;
        }

        private Bitmap CreateHighlightedImage(Bitmap baseImage, Config opts)
        {
            var resultImage = new Bitmap(baseImage);
            resultImage.Brighten(opts);
            return resultImage;
        }
    }
}