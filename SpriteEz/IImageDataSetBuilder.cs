using System.Collections.Generic;

namespace SpriteEz
{
    internal interface IImageDataSetBuilder
    {
        ImageDataSet Build(List<ImgFile> imgFiles, Config config);
    }
}