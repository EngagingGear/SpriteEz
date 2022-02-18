using System.Collections.Generic;
using System.Drawing;

namespace SpriteEz
{
    internal class ProcessedImageData
    {
        public ProcessedImageData(Bitmap spriteBitmap, List<CssEntry> cssEntries, ImageDescriptor imageDescriptor,
            string cssFileName, string spriteFileName = null)
        {
            SpriteBitmap = spriteBitmap;
            CssEntries = cssEntries;
            ImageDescriptor = imageDescriptor;
            CssFileName = cssFileName;
            SpriteFileName = spriteFileName;
        }

        public Bitmap SpriteBitmap { get; }
        public List<CssEntry> CssEntries { get; }
        public ImageDescriptor ImageDescriptor { get; }
        public bool IsScaled => ImageDescriptor?.ScaleDescriptor?.MediaBreakpointConstraint != null;
        public string CssFileName { get; }
        public string SpriteFileName { get; }
    }
}