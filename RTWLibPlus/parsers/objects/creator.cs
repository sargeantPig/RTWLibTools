using System;
using System.Collections.Generic;
using System.Text;
using static RTWLibPlus.parsers.DepthParse;

namespace RTWLibPlus.parsers.objects
{
    public static class Creator
    {
        public static ObjectCreator DScreator = (value, tag, depth) => new DSObj(tag, value, depth);
        public static ObjectCreator EDBcreator = (value, tag, depth) => new EDBObj(tag, value, depth);
        public static ObjectCreator BaseCreator = (value, tag, depth) => new baseObj(tag, value, depth);
    }
}
