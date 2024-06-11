namespace RTWLibPlus.parsers.objects;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.configs.whiteSpace;
using System;
using System.Linq;

public class DSObj : ArrayObj, IBaseObj
{
    private static readonly string[] ApplyDepthToNonArrayAt = new string[3] { "playable", "unlockable", "nonplayable" };
    private static readonly string TerminateNonArrayDepthAt = "end";

    private static bool applyNonArrayDepth;

    public DSObj(string tag, string value, int depth) :
        base(tag, value, depth)
    {
        WSConfigFactory factory = new();
        this.WhiteSpaceConfig = WSConfigFactory.Create_DR_DS_SMF_WhiteSpace();
        string[] tagS = tag.Split(this.WhiteSpaceConfig.WhiteChar);
        this.Ident = tagS[0];
    }

    public DSObj() { }

    public override IBaseObj Copy()
    {
        DSObj copy = new()
        {
            WhiteSpaceChar = this.WhiteSpaceConfig.WhiteChar,
            Depth = this.Depth,
            Items = this.Items.DeepCopy(),
            WhiteSpaceMultiplier = this.WhiteSpaceConfig.WhiteDepthMultiplier,
            Tag = this.Tag,
            Value = this.Value,
            Ident = this.Ident,
            NewLinesAfter = this.NewLinesAfter,
            LastOfGroup = this.LastOfGroup
        };
        return copy;
    }
    public override string Output()
    {
        string output = string.Empty;
        int wDepth = this.Depth;
        this.CheckForNonArrayTerminate();
        output = this.GetTagValue(wDepth);
        output = this.ChildOutput(output);
        this.CheckForNonArray();

        output = this.IfResource(output);
        output = this.IfRelative(output);
        output = this.IfCharacterRecord(output);
        output += GetNewLine(this.NewLinesAfter);
        return output;
    }

    private string ChildOutput(string output)
    {
        if (this.GetItems().Count > 0)
        {
            output += this.OpenBrackets();
            foreach (DSObj item in this.GetItems())
            {
                output += item.Output();
            }
            output += this.CloseBrackets();
        }

        return output;
    }
    //'           '
    /// <summary>
    /// /private string IfLastInGroup
    /// </summary>

    private string IfResource(string output)
    {

        if (this.Ident == "resource")
        {
            string[] splitData = this.Value.Split(',', StringSplitOptions.RemoveEmptyEntries);
            splitData = splitData.TrimAll();
            string resource = string.Format("{0}{1}{2},{3}{4},{5}{6}{7}",
                this.Tag,
                Format.GetWhiteSpace(this.Tag, 25, ' '),
                splitData[0],
                Format.GetWhiteSpace("", 9, ' '),
                splitData[1],
                Format.GetWhiteSpace(splitData[1], 5, ' '),
                splitData[2],
                Format.UniversalNewLine());
            return resource;
        }
        else
        {
            return output;
        }
    }

    private string IfRelative(string output)
    {
        if (this.Ident == "relative")
        {
            string[] splitData = this.Value.Split(',', StringSplitOptions.RemoveEmptyEntries);
            splitData = splitData.TrimAll();
            int i = 0;
            string formatted = string.Empty;
            foreach (string str in splitData)
            {
                if (i == splitData.Length - 1 && i == 2)
                {
                    formatted += string.Format("{0}{1}{2}", Format.GetWhiteSpace("", 2, '\t'), str, Format.UniversalNewLine());
                }
                else if (i == 0)
                {
                    formatted += string.Format("{0} \t{1},", this.Tag, str);
                }
                else if (i == 1)
                {
                    formatted += string.Format(" \t{0},", str);
                }
                else if (i == 2)
                {
                    formatted += string.Format("\t\t{0},", str);
                }
                else if (i == splitData.Length - 1)
                {
                    formatted += string.Format("\t{0}{1}", str, Format.UniversalNewLine());
                }
                else
                {
                    formatted += string.Format("\t{0},", str);
                }

                i++;
            }

            return formatted;
        }
        else
        {
            return output;
        }
    }

    private string IfCharacterRecord(string output)
    {
        if (this.Ident == "character_record")
        {
            string[] splitData = this.Value.Split(',', StringSplitOptions.RemoveEmptyEntries);
            splitData = splitData.TrimAll();
            int i = 0;
            string formatted = string.Empty;
            foreach (string str in splitData)
            {
                if (i == 0 && str == "female")
                {
                    formatted += string.Format("{0} \t{1},", this.Tag, str);
                }
                else if (i == 0 && str == "male")
                {
                    formatted += string.Format("{0} \t{1},", this.Tag, str);
                }
                else if (i == 0)
                {
                    formatted += string.Format("{0} {1},", this.Tag, str);
                }
                else if (i == 1 && splitData[i - 1] != "female" && splitData[i - 1] != "male")
                {
                    formatted += string.Format(" \t{0},", str);
                }
                else if (i == splitData.Length - 1)
                {
                    formatted += string.Format(" {0}{1}", str, Format.UniversalNewLine());
                }
                else
                {
                    formatted += string.Format(" {0},", str);
                }

                i++;
            }

            return formatted;
        }
        else
        {
            return output;
        }
    }

    private void CheckForNonArray()
    {
        if (ApplyDepthToNonArrayAt.Contains(this.Tag))
        {
            applyNonArrayDepth = true;
        }
    }

    private void CheckForNonArrayTerminate()
    {
        if (this.Tag == TerminateNonArrayDepthAt)
        {
            applyNonArrayDepth = false;
        }
    }

    private string GetTagValue(int wDepth)
    {
        string output;
        output = this.GetCorrectFormat(wDepth);
        return output;
    }

    private string GetCorrectFormat(int wDepth)
    {
        string output;

        output = this.NormalFormat(this.WhiteSpaceChar, wDepth);
        output = this.IfTagIsValue(output, wDepth);
        output = this.IfApplyingNonArrayDepth(output);
        return output;
    }

    private string IfApplyingNonArrayDepth(string output)
    {
        if (applyNonArrayDepth)
        {
            return this.GetTabbedLine(1) + output;
        }
        else
        {
            return output;
        }
    }

    private string IfTagIsValue(string output, int wDepth)
    {
        if (this.Tag == this.Value)
        {
            output = this.IgnoreValue(this.WhiteSpaceChar, wDepth);
        }
        return output;
    }

    private string GetTabbedLine(int end) => Format.GetWhiteSpace("", end, this.WhiteSpaceChar);

    private static string GetNewLine(int end) => Format.GetWhiteSpace("", end, Format.UniversalNewLine());

    public bool LastOfGroup { get; set; }

}
