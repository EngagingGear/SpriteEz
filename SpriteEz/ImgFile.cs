// ReSharper disable once CheckNamespace
namespace SpriteEzNs
{
    /// <summary>
    /// Class describing each image to be included.
    /// </summary>
    public class ImgFile
    {
        /// <summary>
        /// The CSS image file name
        /// </summary>
        public string ImgName;

        /// <summary>
        /// Filename of the normal image file
        /// </summary>
        public string NormalImgFilename;

        /// <summary>
        /// Filename of the gray image file or null
        /// </summary>
        public string GrayImageFilename;

        /// <summary>
        /// Filename of the highlight file or null
        /// </summary>
        public string HighlightImgFilename;
    }
}
