namespace RTWLibPlus.parsers.objects;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.configs.whiteSpace;
using System;
using System.Linq;

public class EDBObj : ArrayObj, IBaseObj
{

    private static readonly string[] AlwaysArrays = ["plugins", "upgrades"];
    private static readonly string[] DoubleSpace = ["construction", "cost"];
    private static readonly string[] DoubleSpaceEnding = ["levels"];
    private static readonly string[] WhiteSpaceSwap = ["requires", "temple"];

    public EDBObj(string tag, string value, int depth) :
        base(tag, value, depth)
    {
        WSConfigFactory factory = new();
        this.WhiteSpaceConfig = WSConfigFactory.CreateEDBWhiteSpace();
        this.Ident = this.Tag.Split(this.WhiteSpaceChar)[0];
    }

    public EDBObj() { }
    public override IBaseObj Copy()
    {
        EDBObj copy = new()
        {
            WhiteSpaceChar = this.WhiteSpaceChar,
            Depth = this.Depth,
            Items = this.Items.DeepCopy(),
            WhiteSpaceMultiplier = this.WhiteSpaceMultiplier,
            Tag = this.Tag,
            Value = this.Value,
            Ident = this.Ident,
            NewLinesAfter = this.NewLinesAfter
        };
        return copy;
    }


    public override string Output()
    {
        string output = string.Empty;

        int wDepth = 4 * this.Depth;
        output = this.GetTagValue(wDepth);
        output = this.ChildOutput(output);

        if (this.Tag == "alias")
        {
            output += Format.UniversalNewLine() + Format.UniversalNewLine();
        }

        if (this.Tag == "tags")
        {
            output += Format.UniversalNewLine();
        }

        return output;
    }

    private string ChildOutput(string output)
    {
        if (this.GetItems().Count > 0 || AlwaysArrays.Contains(this.Tag))
        {
            output += this.OpenBrackets();
            foreach (EDBObj item in this.GetItems())
            {
                output += item.Output();
            }
            output += this.CloseBrackets();
        }

        return output;
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

        output = this.NormalFormat(' ', wDepth);
        output = this.IfTagIsValue(output, wDepth);
        output = this.IfInDoubleSpace(output, wDepth);
        output = this.IfInDoubleSpaceEnding(output, wDepth);
        output = this.IfWhiteSpaceSwap(output);

        return output;
    }

    private string IfInDoubleSpaceEnding(string output, int wDepth)
    {
        if (DoubleSpaceEnding.Contains(this.Tag))
        {
            output = this.GetDoubleSpaceEnding(wDepth);
        }
        return output;
    }


    private string IfInDoubleSpace(string output, int wDepth)
    {
        if (DoubleSpace.Contains(this.Tag))
        {
            output = this.GetDoubleSpaceBetweenTagValue(wDepth);
        }
        return output;
    }


    private string IfTagIsValue(string output, int wDepth)
    {
        if (this.Tag == this.Value)
        {
            output = this.IgnoreValue(' ', wDepth);
        }
        return output;
    }

    private string IfWhiteSpaceSwap(string output)
    {
        if (WhiteSpaceSwap.Contains(this.Tag))
        {
            output = this.WhiteSpaceCheckKeyIsValue();
        }
        return output;
    }

    private string WhiteSpaceCheckKeyIsValue()
    {
        string output;
        if (this.Tag != this.Value)
        {
            output = this.NormalFormat('\t', 1);
        }
        else
        {
            output = this.IgnoreValue('\t', 1);
        }

        return output;
    }
    private string GetDoubleSpaceBetweenTagValue(int end) => string.Format("{0}{1}  {2}{3}",
            Format.GetWhiteSpace("", end, this.WhiteSpaceChar),
            this.Tag, this.Value,
            Format.UniversalNewLine());

    private string GetDoubleSpaceEnding(int end) => string.Format("{0}{1} {2}  {3}",
            Format.GetWhiteSpace("", end, this.WhiteSpaceChar),
            this.Tag, this.Value,
            Format.UniversalNewLine());

}

