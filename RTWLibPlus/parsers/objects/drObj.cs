using RTWLibPlus.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.parsers.objects
{
    public class DRObj : baseObj, IbaseObj
    {
        public static char whiteSpace = '\t';
        public static int whiteSpaceMultiplier = 1;

        public DRObj(string tag, string value, int depth) :
            base(tag, value, depth)
        {
            whiteChar = whiteSpace;
            whiteDepthMultiplier = whiteSpaceMultiplier;
            Ident = Tag.Split(whiteChar)[0];
        }

    }
}
