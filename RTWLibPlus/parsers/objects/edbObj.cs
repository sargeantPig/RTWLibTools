﻿using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RTWLibPlus.parsers.DepthParse;

namespace RTWLibPlus.parsers.objects
{
    public class EDBObj : baseObj, IbaseObj
    {

        private static string[] AlwaysArrays = new string[] { "plugins", "upgrades" };
        private static string[] DoubleSpace = new string[] { "construction", "cost" };
        private static string[] DoubleSpaceEnding = new string[] { "levels" };
        private static string[] WhiteSpaceSwap = new string[] { "requires", "temple" };

        private static char whiteSpace = ' ';
        private static int whiteSpaceMultiplier = 4;

        public EDBObj(string tag, string value, int depth) :
            base(tag, value, depth)
        {
            whiteChar = whiteSpace;
            whiteDepthMultiplier = whiteSpaceMultiplier;
            Ident = Tag.Split(whiteChar)[0];
        }

        public EDBObj() { }
        new public IbaseObj Copy()
        {
            EDBObj copy = new EDBObj();
            copy.whiteChar = whiteSpace;
            copy.depth = depth;
            copy.items = items.DeepCopy();
            copy.whiteDepthMultiplier = whiteSpaceMultiplier;
            copy.Tag = Tag;
            copy.Value = Value;
            copy.Ident = Ident;
            copy.newLinesAfter = newLinesAfter;
            return copy;
        }


        new public string Output()
        {
            string output = string.Empty;

            int wDepth = 4 * depth;
            output = GetTagValue(wDepth);
            output = ChildOutput(output);

            if (Tag == "alias")
                output += Environment.NewLine + Environment.NewLine;

            if (Tag == "tags")
                output += Environment.NewLine;

            return output;
        }

        new private string ChildOutput(string output)
        {
            if (GetItems().Count > 0 || AlwaysArrays.Contains(Tag))
            {
                output += OpenBrackets();
                foreach (EDBObj item in GetItems())
                {
                    output += item.Output();
                }
                output += CloseBrackets();
            }

            return output;
        }

        private string GetTagValue(int wDepth)
        {
            string output;
            output = GetCorrectFormat(wDepth);
            return output;
        }

        private string GetCorrectFormat(int wDepth)
        {
            string output;

            output = NormalFormat(' ', wDepth);
            output = IfTagIsValue(output, wDepth);
            output = IfInDoubleSpace(output, wDepth);
            output = IfInDoubleSpaceEnding(output, wDepth);
            output = IfWhiteSpaceSwap(output, wDepth);

            return output;
        }

        private string IfInDoubleSpaceEnding(string output, int wDepth)
        {
            if (DoubleSpaceEnding.Contains(Tag))
            {
                output = GetDoubleSpaceEnding(wDepth);
            }
            return output;
        }


        private string IfInDoubleSpace(string output, int wDepth)
        {
            if (DoubleSpace.Contains(Tag))
            {
                output = GetDoubleSpaceBetweenTagValue(wDepth);
            }
            return output;
        }


        private string IfTagIsValue(string output, int wDepth)
        {
            if (Tag == Value)
            {
                output = IgnoreValue(' ', wDepth);
            }
            return output;
        }

        private string IfWhiteSpaceSwap(string output, int wDepth)
        {
            if (WhiteSpaceSwap.Contains(Tag))
            {
                output = WhiteSpaceCheckKeyIsValue();
            }
            return output;
        }

        private string WhiteSpaceCheckKeyIsValue()
        {
            string output;
            if (Tag != Value)
                output = NormalFormat('\t', 1);
            else output = IgnoreValue('\t', 1);
            return output;
        }

    }
}
