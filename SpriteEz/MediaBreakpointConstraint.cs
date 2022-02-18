namespace SpriteEz
{
    public class MediaBreakpointConstraint
    {
        public const int TopMaxWidth = 10000;

        public MediaBreakpointConstraint(int minWidth, int maxWidth = 0)
        {
            MinWidth = minWidth;
            MaxWidth = maxWidth > 0 ? maxWidth : TopMaxWidth;
        }

        public int MinWidth { get; set; }
        public int MaxWidth { get; set; }
        public string CssFileName { get; set; }

        public override string ToString()
        {
            return $"@import url('./{CssFileName}') (min-width: {MinWidth}px) and (max-width: {MaxWidth}px);";
        }
    }
}