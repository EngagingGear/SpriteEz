namespace SpriteEz
{
    internal class ScaleDescriptor
    {
        public ScaleDescriptor(string key, double scaleFactor, MediaBreakpointConstraint mediaBreakpointConstraint)
        {
            Key = key;
            ScaleFactor = scaleFactor;
            MediaBreakpointConstraint = mediaBreakpointConstraint;
        }

        public string Key { get; }
        public double ScaleFactor { get; }
        public MediaBreakpointConstraint MediaBreakpointConstraint { get; }
    }
}