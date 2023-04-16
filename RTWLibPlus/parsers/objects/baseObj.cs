using RTWLibPlus.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTWLibPlus.parsers.objects
{
    public class baseObj
    {
        public static string[] AlwaysArrays = new string[0];
        public static string[] DoubleSpace = new string[0];
        public static string[] DoubleSpaceEnding = new string[0];
        public static string[] WhiteSpaceSwap = new string[0];

        private List<baseObj> items = new List<baseObj>();

        public List<baseObj> Items { get => items; set => items = value; }
        string Tag { get; set; } = string.Empty;
        string Value { get; set; } = string.Empty;
        int depth { get; set; }

        public baseObj(string tag, string value, int depth)
        {
            this.Tag = tag;
            this.Value = value;
            this.depth = depth;
        }

        public string Output()
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

        private string ChildOutput(string output)
        {
            if (Items.Count > 0 || AlwaysArrays.Contains(Tag))
            {
                output += OpenBrackets();
                foreach (baseObj item in Items)
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
            if (WhiteSpaceSwap.Contains(Tag))
            {
                output = CheckKeyIsValue();
            }
            else
            {
                output = GetCorrectFormat(wDepth);
            }
            return output;
        }

        private string GetCorrectFormat(int wDepth)
        {
            string output;
            if (Tag == Value)
                output = IgnoreValue(' ', wDepth);
            else if (DoubleSpace.Contains(Tag))
                output = GetDoubleSpaceBetweenTagValue(wDepth);
            else if (DoubleSpaceEnding.Contains(Tag))
                output = GetDoubleSpaceEnding(wDepth);
            else output = NormalFormat(' ', wDepth);
            return output;
        }

        private string CheckKeyIsValue()
        {
            string output;
            if (Tag != Value)
                output = NormalFormat('\t', 1);
            else output = IgnoreValue('\t', 1);
            return output;
        }

        private string CloseBrackets()
        {
            return String.Format("{0}}}{1}", 
                Format.GetWhiteSpace("", 4 * depth, ' '), 
                Environment.NewLine);
        }

        private string OpenBrackets()
        {
            return String.Format("{0}{{{1}",
                Format.GetWhiteSpace("", 4 * depth, ' '), 
                Environment.NewLine);
        }

        private string NormalFormat(char whiteSpace, int end)
        {
            return String.Format("{0}{1} {2}{3}", 
                Format.GetWhiteSpace("", end, whiteSpace), 
                Tag, Value, 
                Environment.NewLine);
        }

        private string IgnoreValue(char whiteSpace, int end)
        {
            return String.Format("{0}{1}{2}", 
                Format.GetWhiteSpace("", end, whiteSpace), 
                Tag, 
                Environment.NewLine);
        }

        private string GetDoubleSpaceBetweenTagValue(int end)
        {
            return String.Format("{0}{1}  {2}{3}", 
                Format.GetWhiteSpace("", end, ' '), 
                Tag, Value, 
                Environment.NewLine);
        }

        private string GetDoubleSpaceEnding(int end)
        {
            return String.Format("{0}{1} {2}  {3}", 
                Format.GetWhiteSpace("", end, ' '), 
                Tag, Value, 
                Environment.NewLine);
        }
    }
}
