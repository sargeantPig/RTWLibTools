using RTWLibPlus.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.parsers.objects
{
    public class DRObj : baseObj, IbaseObj
    {
        private static char whiteSpace = '\t';
        private static int whiteSpaceMultiplier = 1;

        public DRObj(string tag, string value, int depth) :
            base(tag, value, depth)
        {
            whiteChar = whiteSpace;
            whiteDepthMultiplier = whiteSpaceMultiplier;
            Ident = Tag.Split(whiteChar)[0];
        }

    }
}
