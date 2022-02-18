using System.Drawing;

// ReSharper disable once CheckNamespace
namespace SpriteEz
{
    /// <summary>
    ///     This class represents the triplet of images. If necessary it
    ///     generates highlighted and disabled images.
    /// </summary>
    internal class ImgBitmap
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="files">The file names</param>
        /// <param name="normalImg"></param>
        /// <param name="disabledImage"></param>
        /// <param name="highlightImage"></param>
        public ImgBitmap(ImgFile files, Bitmap normalImg, Bitmap disabledImage, Bitmap highlightImage)
        {
            FileNames = files;
            NormalImg = normalImg;
            DisabledImage = disabledImage;
            HighlightImage = highlightImage;
        }

        /// <summary>
        ///     The file entry.
        /// </summary>
        public ImgFile FileNames { get; }

        /// <summary>
        ///     The normal image
        /// </summary>
        public Bitmap NormalImg { get; }

        /// <summary>
        ///     The gray image, or null if not required.
        /// </summary>
        public Bitmap DisabledImage { get; }

        /// <summary>
        ///     The brightened image, or null if not required.
        /// </summary>
        public Bitmap HighlightImage { get; }
    }
}