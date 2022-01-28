using System.Collections.Generic;

namespace SpriteEz
{
    internal class ImageDescriptor
    {
        public ImageDescriptor(IList<ImgBitmap> bitmaps, ScaleDescriptor scaleDescriptor)
        {
            Bitmaps = bitmaps;
            ScaleDescriptor = scaleDescriptor;
        }

        public IList<ImgBitmap> Bitmaps { get; }
        public ScaleDescriptor ScaleDescriptor { get; }
    }
}