using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RTWLibPlus.parsers.DepthParse;

namespace RTWLibPlus.edb
{
    public class EDBObj : baseObj, IbaseObj
    {
        //baseObj baseData;
        new public static ObjectCreator creator = (value, tag, depth) => new EDBObj(tag, value, depth);

        public static string[] AlwaysArrays = new string[0];
        public static string[] DoubleSpace = new string[0];
        public static string[] DoubleSpaceEnding = new string[0];
        public static string[] WhiteSpaceSwap = new string[0];

        public static char whiteSpace = ' ';
        public static int whiteSpaceMultiplier = 4;

        public EDBObj(string tag, string value, int depth) : 
            base(tag, value, depth) {

            base.whiteChar = whiteSpace;
            base.whiteDepthMultiplier = whiteSpaceMultiplier;
        }

        public EDBObj() { }

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

        new public List<IbaseObj> GetItems()
        {
            return base.GetItems();
        }
        new public void SetItems(List<IbaseObj> baseObjs)
        {
            SetItems(baseObjs);
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

