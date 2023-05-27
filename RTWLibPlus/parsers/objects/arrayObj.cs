using RTWLibPlus.helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.parsers.objects
{
    public abstract class ArrayObj : BaseObj
    {
        public ArrayObj() { }
        public ArrayObj(string tag, string value, int depth) : base(tag, value, depth)
        {

        }

        public string CloseBrackets()
        {
            return String.Format("{0}}}{1}",
                Format.GetWhiteSpace("", wsConfig.WhiteDepthMultiplier * Depth,wsConfig.WhiteChar),
                Environment.NewLine);
        }

        public string OpenBrackets()
        {
            return String.Format("{0}{{{1}",
                Format.GetWhiteSpace("", wsConfig.WhiteDepthMultiplier * Depth, wsConfig.WhiteChar),
                Environment.NewLine);
        }

    }
}
