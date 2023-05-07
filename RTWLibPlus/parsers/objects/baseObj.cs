using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using System;
using System.Collections.Generic;
using static RTWLibPlus.parsers.DepthParse;

namespace RTWLibPlus.parsers.objects
{
    public class baseObj : IbaseObj
    {
        public char whiteChar = '\t';
        public int whiteDepthMultiplier = 1;
        public static Func<string, int> specialPadding = null;
        public List<IbaseObj> items = new List<IbaseObj>();
        
        public string Tag { get; set; } = string.Empty;
        public string Ident { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int depth { get; set; }

        public int newLinesAfter { get; set; } = 0;

        public baseObj() { }

        public baseObj(string tag, string value, int depth)
        {
            this.Tag = tag;
            this.Value = value;
            this.depth = depth;
            this.Ident = Tag.Split(whiteChar)[0];
        }

        public IbaseObj Copy()
        {
            baseObj copy = new baseObj();
            copy.whiteChar = whiteChar;
            copy.depth = depth;
            copy.items = items.DeepCopy();
            copy.whiteDepthMultiplier = whiteDepthMultiplier;
            copy.Tag = Tag;
            copy.Value = Value;
            copy.Ident = Ident;
            copy.newLinesAfter = newLinesAfter;
            return copy;
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

        public string GetNewLine(int end)
        {
            return Format.GetWhiteSpace("", end, Environment.NewLine);
        }

        public string GetTabbedLine(int end)
        {
            return Format.GetWhiteSpace("", end, whiteChar);
        }
    }   
}
