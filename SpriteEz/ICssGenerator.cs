using System.Collections.Generic;

namespace SpriteEzNs
{
    public interface ICssGenerator
    {
        public List<string> Generate(List<CssEntry> cssEntries, Config config);
    }
}