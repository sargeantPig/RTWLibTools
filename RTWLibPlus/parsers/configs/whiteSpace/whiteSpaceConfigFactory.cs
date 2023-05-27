using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.parsers.configs.whiteSpace
{
    public class WSConfigFactory
    {
        public WhiteSpaceConfig CreateEDBWhiteSpace()
        {
            return new WhiteSpaceConfig(' ', 4);
        }

        public WhiteSpaceConfig CreateEDUWhiteSpace()
        {
            return new WhiteSpaceConfig(' ', 1);
        }

        public WhiteSpaceConfig Create_DR_DS_SMF_WhiteSpace()
        {
            return new WhiteSpaceConfig('\t', 1);
        }
    }
}
