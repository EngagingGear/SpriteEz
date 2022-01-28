using System.Collections.Generic;

namespace SpriteEz
{
    internal class ImageDataSet
    {
        public ImageDataSet(IList<ImageDescriptor> items)
        {
            Items = items;
        }

        public IList<ImageDescriptor> Items { get; }
    }
}