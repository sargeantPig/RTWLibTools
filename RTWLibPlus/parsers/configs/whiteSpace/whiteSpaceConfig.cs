using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.parsers.configs.whiteSpace
{
    public struct WhiteSpaceConfig
    {
        public char WhiteChar { get; set; }
        public int WhiteDepthMultiplier { get; set; }

        public WhiteSpaceConfig(char whiteChar, int whiteDepthMultiplier)
        {
            this.WhiteChar = whiteChar;
            this.WhiteDepthMultiplier = whiteDepthMultiplier;
        }

    }
}
