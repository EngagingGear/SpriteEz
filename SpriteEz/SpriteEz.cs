﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using nQuant;

// ReSharper disable once CheckNamespace
namespace SpriteEzNs
{
    /// <summary>
    ///     Simple class to generate the sprite and css file.
    /// </summary>
    public class SpriteEz
    {
        private static readonly int alphaTransparency = 10;
        private static readonly int alphaFader = 70;

        private readonly Config _config;
        private readonly ICssGenerator _cssGenerator;
        private readonly EmbeddedImageEncoder _embeddedImageEncoder;
        private readonly IHtmlHelpFileGenerator _helpFileGenerator;
        private readonly List<ImgFile> _imgFiles;
        private readonly Logger _logger;

        /// <summary>
        ///     Constructor which does all the work.
        /// </summary>
        /// <param name="imgFiles">Image files to process</param>
        /// <param name="config">Options for generation</param>
        /// <param name="logger"></param>
        public SpriteEz(List<ImgFile> imgFiles, Config config, Logger logger)
        {
            _imgFiles = imgFiles;
            _config = config;
            _logger = logger;
            _cssGenerator = _config.Embedded ? new EmbeddedCssGenerator() : new SpriteCssGenerator();
            _helpFileGenerator = _config.Embedded
                ? new EmbeddedImageHtmlHelpFileGenerator(logger)
                : new SpriteHtmlHelpFileGenerator(logger);
            _embeddedImageEncoder = new EmbeddedImageEncoder();
        }

        public void Generate()
        {
            // The steps are:
            // 1. Various error checks.
            // 2. Create an array of ImgBitmap which contains bitmaps for the three forms.
            // 3. Find the dimensions, width is the sum of all image widths, height is 1-3 depending on
            //    whether gray and highlight are included
            // 4. Create a big enough sprite to contain it.
            // 5. Loop over all images drawing them in the big sprite and saving coordinate information
            // 6. Generate the CSS file using the templates.
            // Please note that the pictures are simply laid out in a grid, which, should they be different
            // sizes leaves hole. However, these holes don't contribute significantly to size because of the
            // image compression.
            //
            // 1. Various error checks.
            if (_imgFiles.Count == 0)
                throw new ArgumentException("At least one image is required");

            // 2. Create an array of ImgBitmap which contains bitmaps for the three forms.
            var bitmaps = _imgFiles.Select(item => new ImgBitmap(item, _config)).ToArray();


            var cssEntries = new List<CssEntry>();
            if (_config.Embedded)
            {
                //generate embedded images data for further processing with css
                cssEntries.AddRange(GeneratedEmbeddedImages(bitmaps));
            }
            else
            {
                var (sprite, generatedCssEntries) = CreateSprite(bitmaps);
                cssEntries.AddRange(generatedCssEntries);

                //save generated sprite image file
                try
                {
                    var destinationPath = FileUtils.GetDestinationPath(_config.OutputDirectory, _config.SpriteImgFile);
                    var imageToSave = _config.Compress ? CompressImage(sprite) : sprite;
                    imageToSave.Save(destinationPath, ImageFormat.Png);
                }
                catch (Exception exception)
                {
                    _logger.Log($"Could not save sprite image: {exception.Message}");
                }
            }

            // Create and save the CSS file using the templates from configuration file
            var cssLines = _cssGenerator.Generate(cssEntries, _config);
            using (var writer =
                new StreamWriter(FileUtils.GetDestinationPath(_config.OutputDirectory, _config.SpriteCssFile)))
            {
                foreach (var cssLine in cssLines) writer.WriteLine(cssLine);
            }

            //generate and save html help file
            if (_config.GenerateHelpFile
                && !string.IsNullOrWhiteSpace(_config.HelpFile)
                && !string.IsNullOrWhiteSpace(_config.SpriteCssFile))
                _helpFileGenerator.GenerateFile(cssEntries, _config);
        }

        private CssEntry CreateCssEntry(string imgName)
        {
            return new(imgName);
        }

        private (Bitmap sprite, List<CssEntry> cssEntries) CreateSprite(ImgBitmap[] bitmaps)
        {
            // Find the dimensions, width is the sum of all image widths, height is 1-3 depending on
            // whether gray and highlight are included
            var width = bitmaps.Sum(item => item.NormalImg.Width);
            var height = bitmaps.Max(item => item.NormalImg.Height);
            var heightMultiplier = 3;
            var disabledRow = 1;
            var highlightRow = 2;
            // Note we insert a highlight or disabled row if the options indicate so or if there are any
            // highlight or disabled images supplied.
            if (_config.GenerateWithFilters || !_config.GenerateHighlight &&
                bitmaps.FirstOrDefault(item => item.HighlightImage != null) == null)
            {
                highlightRow = -1;
                heightMultiplier--;
            }

            if (_config.GenerateWithFilters || !_config.GenerateDisabled &&
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

            return (sprite, cssEntries);
        }

        private List<CssEntry> GeneratedEmbeddedImages(ImgBitmap[] images)
        {
            var result = new List<CssEntry>();
            foreach (var image in images)
            {
                var cssEntry = CreateCssEntry(image.FileNames.ImgName);
                if (_config.Compress) _logger.Log("Compressing embedded image");

                var normalImage = _config.Compress ? CompressImage(image.NormalImg) : image.NormalImg;
                cssEntry.NormalImageCssText = _embeddedImageEncoder.Encode(normalImage);
                cssEntry.NormalImagePos = new Rectangle(0, 0, normalImage.Width, normalImage.Height);

                if (image.DisabledImage != null)
                {
                    var disabledImage
                        = _config.Compress ? CompressImage(image.DisabledImage) : image.DisabledImage;
                    cssEntry.DisabledImagePos = new Rectangle(0, 0, disabledImage.Width, disabledImage.Height);
                    cssEntry.DisabledImageCssText = _embeddedImageEncoder.Encode(disabledImage);
                }

                if (image.HighlightImage != null)
                {
                    cssEntry.HighlightImagePos = new Rectangle(0, 0, image.NormalImg.Width, image.NormalImg.Height);
                    var highlightImage
                        = _config.Compress ? CompressImage(image.HighlightImage) : image.HighlightImage;
                    cssEntry.DisabledImagePos = new Rectangle(0, 0, highlightImage.Width, highlightImage.Height);
                    cssEntry.HighlightedImageCssText = _embeddedImageEncoder.Encode(highlightImage);
                }

                result.Add(cssEntry);
            }

            return result;
        }

        private Image CompressImage(Bitmap image)
        {
            var compressor = new WuQuantizer();
            var compressedImage = compressor.QuantizeImage(image, alphaTransparency, alphaFader);
            return compressedImage;
        }
    }
}