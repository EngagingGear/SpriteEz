using System.Collections.Generic;

namespace SpriteEz
{
    public interface IHtmlHelpFileGenerator
    {
        void GenerateFile(List<string> imageNames, Config config, string cssFileName);
    }
}