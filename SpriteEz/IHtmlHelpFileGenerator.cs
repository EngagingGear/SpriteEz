using System.Collections.Generic;

namespace SpriteEzNs
{
    public interface IHtmlHelpFileGenerator
    {
        void GenerateFile(List<CssEntry> cssEntries, Config config);
    }
}