using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.parsers.objects
{
    public class EDUObj : baseObj, IbaseObj
    {
        private static char whiteSpace = ' ';
        private static int whiteSpaceMultiplier = 1;

        public bool endOfUnit = false;

        public EDUObj(string tag, string value, int depth) :
            base(tag, value, depth)
        {
            whiteChar = whiteSpace;
            whiteDepthMultiplier = whiteSpaceMultiplier;
            Ident = Tag.Split(whiteChar)[0];
        }

        public EDUObj() { }

        new public IbaseObj Copy()
        {
            EDUObj copy = new EDUObj();
            copy.whiteChar = whiteSpace;
            copy.depth = depth;
            copy.items = items.DeepCopy();
            copy.whiteDepthMultiplier = whiteSpaceMultiplier;
            copy.Tag = Tag;
            copy.Value = Value;
            copy.Ident = Ident;
            copy.newLinesAfter = newLinesAfter;
            copy.endOfUnit = endOfUnit;
            return copy;
        }

        new public string Output()
        {
            string output = string.Empty;

            if (Tag == "rebalance_statblock")
                output = FormatLineDropValue();
            else if(Tag == "is_female")
                output = FormatLineWithWhitespaceDropValue();
            else output = FormatLine();

            if (endOfUnit)
                output += Environment.NewLine;


            return output;
        }


        private string FormatLine()
        {
            return string.Format("{0}{1}{2}{3}", Tag, Format.GetWhiteSpace(Tag, 20, ' '), Value, Environment.NewLine);
        }

        private string FormatLineDropValue()
        {
            return string.Format("{0} {1}", Tag, Environment.NewLine);
        }

        private string FormatLineWithWhitespaceDropValue()
        {
            return string.Format("{0}{1}{2}", Tag, Format.GetWhiteSpace(Tag, 20, ' '), Environment.NewLine);
        }

    }
}
