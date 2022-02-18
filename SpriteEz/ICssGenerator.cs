using System.Collections.Generic;

namespace SpriteEz
{
    public interface ICssGenerator
    {
        public List<string> Generate(List<CssEntry> cssEntries, Config config, string spriteFileName = null);
    }
}