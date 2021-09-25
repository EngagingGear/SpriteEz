// ReSharper disable once CheckNamespace

using System.IO;

namespace SpriteEzNs
{ 
    /// <summary>
    /// Generation options
    /// </summary>
    public class Config
    {
        /// <summary>
        /// If true, generate a highlighted image for all icons.
        /// </summary>
        public bool GenerateHighlight;

        /// <summary>
        /// If true, generate a disabled image for all icons.
        /// </summary>
        public bool GenerateDisabled;

        /// <summary>
        /// How much to auto brighten the image.
        /// </summary>
        public double HighlightMultiplier;

        /// <summary>
        /// How much to darken or brighten the grayed image
        /// </summary>
        public double DisableMultiplier;

        /// <summary>
        /// Name of the sprite file to generate
        /// </summary>
        public string SpriteImgFile;

        /// <summary>
        /// Name of the css file to generate
        /// </summary>
        public string SpriteCssFile;

        /// <summary>
        /// Templates used for the CSS line. $$NAME$$ is replaced by the image name,
        /// $$CSS$$ is replaced by the CSS. Note this does not include {}
        /// </summary>
        public string NormalCssTemplate;

        /// <summary>
        /// Templates used for the CSS line. $$NAME$$ is replaced by the image name,
        /// $$CSS$$ is replaced by the CSS. Note this does not include {}
        /// </summary>
        public string DisabledCssTemplate;
        
        /// <summary>
        /// Templates used for the CSS line. $$NAME$$ is replaced by the image name,
        /// $$CSS$$ is replaced by the CSS. Note this does not include {}
        /// </summary>
        public string HighlightCssTemplate;

        /// <summary>
        /// Directory to copy the component and derived parts
        /// </summary>
        public string DerivedDirectory;

        /// <summary>
        /// Suffix used to determine if the image is a highlighted version of the suffix-less
        /// image. For example, home.png and home_h.png would be considered related
        /// </summary>
        public string HighlightSuffix = "_h";
        /// <summary>
        /// Suffix used to determine if the image is a disabled version of the suffix-less
        /// image. For example, home.png and home_d.png would be considered related
        /// </summary>
        public string DisabledSuffix = "_d";

        public bool GenerateHelpFile;

        public string HelpFile;

        public double GrayScalePixelThrehold;

        public bool GenerateWithFilters;
        public string ImageClass;
        public string ImageTemplate;
        public string OutputDirectory;
    }
}
