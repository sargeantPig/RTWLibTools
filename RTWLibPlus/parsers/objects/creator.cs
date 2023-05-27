using System;
using System.Collections.Generic;
using System.Text;
using static RTWLibPlus.parsers.DepthParse;

namespace RTWLibPlus.parsers.objects
{
    public static class Creator
    {
        public static ObjectCreator EDUcreator = (value, tag, depth) => new EDUObj(tag, value, depth);
        public static ObjectCreator DRcreator = (value, tag, depth) => new DRObj(tag, value, depth);
        public static ObjectCreator DScreator = (value, tag, depth) => new DSObj(tag, value, depth);
        public static ObjectCreator EDBcreator = (value, tag, depth) => new EDBObj(tag, value, depth);
        public static ObjectCreator SMFcreator = (value, tag, depth) => new SMFObj(tag, value, depth);
    }
}
