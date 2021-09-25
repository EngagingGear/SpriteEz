using System.Drawing;

// ReSharper disable once CheckNamespace
namespace SpriteEzNs
{
    /// <summary>
    /// Contains an entry for css, including a rectangle for each type of image and the name.
    /// Note if the image is omited, then the rectangle is null.
    /// </summary>
    public class CssEntry
    {
        /// <summary>
        ///  Name of the image to use in CSS definitions. Usually the normal file basename
        /// </summary>
        public string ImgName;
        /// <summary>
        ///  Position of the normal image
        /// </summary>
        public Rectangle NormalImagePos;
        /// <summary>
        /// Position of the highlight image or null
        /// </summary>
        public Rectangle HighlightImagePos;
        /// <summary>
        /// Position of the disabled image or null
        /// </summary>
        public Rectangle DisabledImagePos;

        public string NormalImageCssText;
        public string DisabledImageCssText;
        public string HighlightedImageCssText;
    }
}
