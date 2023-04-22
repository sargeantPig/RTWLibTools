using RTWLibPlus.edb;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RTWLibPlus.parsers.DepthParse;

namespace RTWLibPlus.parsers.objects
{
    public class baseObj : IbaseObj
    {
        public static ObjectCreator creator = (value, tag, depth) => new baseObj(tag, value, depth);

        public char whiteChar = '\t';
        public int whiteDepthMultiplier = 1;
        public static Func<string, int> specialPadding = null;
        private List<IbaseObj> items = new List<IbaseObj>();
        
        public string Tag { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int depth { get; set; }

        public baseObj() { }

        public baseObj(string tag, string value, int depth)
        {
            this.Tag = tag;
            this.Value = value;
            this.depth = depth;
        }

        public string Output()
        {
            string output = string.Empty;

            int wDepth = whiteDepthMultiplier * depth;
            output = GetTagValue(wDepth);
            output = ChildOutput(output);

            return output;
        }

        public string ChildOutput(string output)
        {
            if (GetItems().Count > 0)
            {
                output += OpenBrackets();
                foreach (baseObj item in GetItems())
                {
                    output += item.Output();
                }
                output += CloseBrackets();
            }

            return output;
        }

        private string GetCorrectFormat(int wDepth)
        {
            string output;

            output = NormalFormat(whiteChar, wDepth);
            output = IfTagIsValue(output, wDepth);

            return output;
        }

        public List<IbaseObj> GetItems()
        {
            return items;
        }
        public void SetItems(List<IbaseObj> baseObjs)
        {
            items = baseObjs;
        }

        private string IfTagIsValue(string output, int wDepth)
        {
            if (Tag == Value)
            {
                output = IgnoreValue(whiteChar, wDepth);
            }
            return output;
        }

        private string GetTagValue(int wDepth)
        {
            string output;
            output = GetCorrectFormat(wDepth);
            return output;
        }

        public string CloseBrackets()
        {
            return String.Format("{0}}}{1}",
                Format.GetWhiteSpace("", whiteDepthMultiplier * depth, whiteChar),
                Environment.NewLine);
        }

        public string OpenBrackets()
        {
            return String.Format("{0}{{{1}",
                Format.GetWhiteSpace("", whiteDepthMultiplier * depth, whiteChar),
                Environment.NewLine);
        }

        public string NormalFormat(char whiteSpace, int end)
        {
            return String.Format("{0}{1} {2}{3}",
                Format.GetWhiteSpace("", end, whiteSpace),
                Tag, Value,
                Environment.NewLine);
        }

        public string IgnoreValue(char whiteSpace, int end)
        {
            return String.Format("{0}{1}{2}",
                Format.GetWhiteSpace("", end, whiteSpace),
                Tag,
                Environment.NewLine);
        }

        public string GetDoubleSpaceBetweenTagValue(int end)
        {
            return String.Format("{0}{1}  {2}{3}",
                Format.GetWhiteSpace("", end, whiteChar),
                Tag, Value,
                Environment.NewLine);
        }

        public string GetDoubleSpaceEnding(int end)
        {
            return String.Format("{0}{1} {2}  {3}",
                Format.GetWhiteSpace("", end, whiteChar),
                Tag, Value,
                Environment.NewLine);
        }
    }   
}
