using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.configs.whiteSpace;
using System;
using System.Linq;

using static RTWLibPlus.parsers.DepthParse;

namespace RTWLibPlus.parsers.objects
{
    public class DSObj : ArrayObj, IbaseObj
    {
        private static string[] applyDepthToNonArrayAt = new string[3] { "playable", "unlockable", "nonplayable" };
        private static string terminateNonArrayDepthAt = "end";

        private static bool applyNonArrayDepth = false;
        public bool lastOfGroup = false;
        public DSObj(string tag, string value, int depth) :
            base(tag, value, depth)
        {
            WSConfigFactory factory = new WSConfigFactory();
            wsConfig = factory.Create_DR_DS_SMF_WhiteSpace();
            var tagS = tag.Split(wsConfig.WhiteChar);
            Ident = tagS[0];
        }

        public DSObj() { }

        public override IbaseObj Copy()
        {
            DSObj copy = new DSObj();
            copy.wsConfig.WhiteChar = wsConfig.WhiteChar;
            copy.Depth = Depth;
            copy.items = items.DeepCopy();
            copy.wsConfig.WhiteDepthMultiplier = wsConfig.WhiteDepthMultiplier;
            copy.Tag = Tag;
            copy.Value = Value;
            copy.Ident = Ident;
            copy.NewLinesAfter = NewLinesAfter;
            copy.lastOfGroup = lastOfGroup;
            return copy;
        }
        public override string Output()
        {
            string output = string.Empty;
            int wDepth = Depth;
            CheckForNonArrayTerminate();
            output = GetTagValue(wDepth);
            output = ChildOutput(output);
            CheckForNonArray();

            output = IfResource(output);
            output = IfRelative(output);
            output = IfCharacterRecord(output);
            output += GetNewLine(NewLinesAfter);
            return output;
        }

        new private string ChildOutput(string output)
        {
            if (GetItems().Count > 0)
            {
                output += OpenBrackets();
                foreach (DSObj item in GetItems())
                {
                    output += item.Output();
                }
                output += CloseBrackets();
            }

            return output;
        }
        //'           '
        /// <summary>
        /// /private string IfLastInGroup
        /// </summary>                                 

        private string IfResource(string output)
        {

            if (Ident == "resource")
            {
                string[] splitData = Value.Split(',', StringSplitOptions.RemoveEmptyEntries);
                splitData = splitData.TrimAll();
                string resource = string.Format("{0}{1}{2},{3}{4},{5}{6}{7}",
                    Tag,
                    Format.GetWhiteSpace(Tag, 25, ' '),
                    splitData[0],
                    Format.GetWhiteSpace("", 9, ' '),
                    splitData[1],
                    Format.GetWhiteSpace(splitData[1], 5, ' '),
                    splitData[2],
                    Environment.NewLine);
                return resource;
            }
            else return output;
        }

        private string IfRelative(string output)
        {
            if (Ident == "relative")
            {
                string[] splitData = Value.Split(',', StringSplitOptions.RemoveEmptyEntries);
                splitData = splitData.TrimAll();
                int i = 0;
                string formatted = string.Empty;
                foreach (string str in splitData)
                {
                    if (i == splitData.Count() - 1 && i == 2)
                        formatted += string.Format("{0}{1}{2}", Format.GetWhiteSpace("", 2, '\t'), str, Environment.NewLine);
                    else if (i == 0)
                        formatted += string.Format("{0} \t{1},", Tag, str);
                    else if (i == 1)
                        formatted += string.Format(" \t{0},", str);
                    else if (i == 2)
                        formatted += string.Format("\t\t{0},", str);
                    else if (i == splitData.Count() - 1)
                        formatted += string.Format("\t{0}{1}", str, Environment.NewLine);
                    else
                        formatted += string.Format("\t{0},", str);
                    i++;
                }

                return formatted;
            }
            else return output;
        }

        private string IfCharacterRecord(string output)
        {
            if (Ident == "character_Record")
            {
                string[] splitData = Value.Split(',', StringSplitOptions.RemoveEmptyEntries);
                splitData = splitData.TrimAll();
                int i = 0;
                string formatted = string.Empty;
                foreach (string str in splitData)
                {
                    if (i == 0 && str == "female")
                        formatted += string.Format("{0} \t{1},", Tag, str);
                    else if (i == 0 && str == "male")
                        formatted += string.Format("{0} \t{1},", Tag, str);
                    else if (i == 0)
                        formatted += string.Format("{0} {1},", Tag, str);
                    else if (i == 1 && splitData[i - 1] != "female" && splitData[i - 1] != "male")
                        formatted += string.Format(" \t{0},", str);
                    else if (i == splitData.Count() - 1)
                        formatted += string.Format(" {0}{1}", str, Environment.NewLine);
                    else
                        formatted += string.Format(" {0},", str);
                    i++;
                }

                return formatted;
            }
            else return output;
        }


        private void CheckForNonArray()
        {
            if (applyDepthToNonArrayAt.Contains(Tag))
                applyNonArrayDepth = true;
        }

        private void CheckForNonArrayTerminate()
        {
            if (Tag == terminateNonArrayDepthAt)
                applyNonArrayDepth = false;
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

            output = NormalFormat(wsConfig.WhiteChar, wDepth);
            output = IfTagIsValue(output, wDepth);
            output = IfApplyingNonArrayDepth(output, wDepth);
            return output;
        }

        private string IfApplyingNonArrayDepth(string output, int wDepth)
        {
            if (applyNonArrayDepth)
                return GetTabbedLine(1) + output;
            else return output;
        }

        private string IfTagIsValue(string output, int wDepth)
        {
            if (Tag == Value)
            {
                output = IgnoreValue(wsConfig.WhiteChar, wDepth);
            }
            return output;
        }

        private string GetTabbedLine(int end)
        {
            return Format.GetWhiteSpace("", end, wsConfig.WhiteChar);
        }

        private string GetNewLine(int end)
        {
            return Format.GetWhiteSpace("", end, Environment.NewLine);
        }

    }
}
