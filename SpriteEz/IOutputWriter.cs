using System.Collections.Generic;

namespace SpriteEz
{
    internal interface IOutputWriter
    {
        void WriteResults(List<ProcessedImageData> results, Config config);
    }
}