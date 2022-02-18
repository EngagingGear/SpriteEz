namespace SpriteEz
{
    /// <summary>
    ///     Generation options
    /// </summary>
    public class Config
    {
        public double AutoSizeLg;
        public double AutoSizeMd;
        public double AutoSizeSm;
        public double AutoSizeXl;

        public double AutoSizeXs;
        public double AutoSizeXxl;

        public bool Compress;

        /// <summary>
        ///     Directory to copy the component and derived parts
        /// </summary>
        public string DerivedDirectory;

        /// <summary>
        ///     Templates used for the CSS line. $$NAME$$ is replaced by the image name,
        ///     $$CSS$$ is replaced by the CSS. Note this does not include {}
        /// </summary>
        public string DisabledCssTemplate;

        /// <summary>
        ///     Suffix used to determine if the image is a disabled version of the suffix-less
        ///     image. For example, home.png and home_d.png would be considered related
        /// </summary>
        public string DisabledSuffix;

        /// <summary>
        ///     How much to darken or brighten the grayed image
        /// </summary>
        public double DisableMultiplier;

        public bool Embedded;

        /// <summary>
        ///     If true, generate a disabled image for all icons.
        /// </summary>
        public bool GenerateDisabled;

        public bool GenerateHelpFile;

        /// <summary>
        ///     If true, generate a highlighted image for all icons.
        /// </summary>
        public bool GenerateHighlight;

        public bool GenerateWithFilters;

        public double GrayScalePixelThrehold;

        public string HelpFile;

        /// <summary>
        ///     Templates used for the CSS line. $$NAME$$ is replaced by the image name,
        ///     $$CSS$$ is replaced by the CSS. Note this does not include {}
        /// </summary>
        public string HighlightCssTemplate;

        /// <summary>
        ///     How much to auto brighten the image.
        /// </summary>
        public double HighlightMultiplier;


        /// <summary>
        ///     Suffix used to determine if the image is a highlighted version of the suffix-less
        ///     image. For example, home.png and home_h.png would be considered related
        /// </summary>
        public string HighlightSuffix;

        public string ImageClass;
        public string ImageTemplate;

        /// <summary>
        ///     Templates used for the CSS line. $$NAME$$ is replaced by the image name,
        ///     $$CSS$$ is replaced by the CSS. Note this does not include {}
        /// </summary>
        public string NormalCssTemplate;

        public string NormalSuffix;
        public string OutputDirectory;

        /// <summary>
        ///     Name of the css file to generate
        /// </summary>
        public string SpriteCssFile;

        /// <summary>
        ///     Name of the sprite file to generate
        /// </summary>
        public string SpriteImgFile;
    }
}