using System.Drawing;

// ReSharper disable once CheckNamespace
namespace SpriteEz
{
    /// <summary>
    ///     Contains an entry for css, including a rectangle for each type of image and the name.
    ///     Note if the image is omitted, then the rectangle is null.
    /// </summary>
    public class CssEntry
    {
        public string DisabledImageCssText;

        /// <summary>
        ///     Position of the disabled image or null
        /// </summary>
        public Rectangle DisabledImagePos;

        public string HighlightedImageCssText;

        /// <summary>
        ///     Position of the highlight image or null
        /// </summary>
        public Rectangle HighlightImagePos;

        public string NormalImageCssText;

        /// <summary>
        ///     Position of the normal image
        /// </summary>
        public Rectangle NormalImagePos;

        public CssEntry(string imgName)
        {
            ImgName = imgName;
        }

        /// <summary>
        ///     Name of the image to use in CSS definitions. Usually the normal file basename
        /// </summary>
        public string ImgName { get; }
    }
}