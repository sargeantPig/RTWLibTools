namespace RTWLibPlus.parsers.objects;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.configs.whiteSpace;

public class EDUObj : BaseObj, IBaseObj
{
    public bool EndOfUnit { get; set; }

    public EDUObj(string tag, string value, int depth) :
        base(tag, value, depth)
    {
        WSConfigFactory factory = new();
        this.WhiteSpaceConfig = WSConfigFactory.Create_EDU_DMB_WhiteSpace();
        this.Ident = this.Tag.Split(this.WhiteSpaceChar)[0];
    }

    public EDUObj() { }

    public override IBaseObj Copy()
    {
        EDUObj copy = new()
        {
            WhiteSpaceChar = this.WhiteSpaceChar,
            Depth = this.Depth,
            Items = this.Items.DeepCopy(),
            WhiteSpaceMultiplier = this.WhiteSpaceMultiplier,
            Tag = this.Tag,
            Value = this.Value,
            Ident = this.Ident,
            NewLinesAfter = this.NewLinesAfter,
            EndOfUnit = this.EndOfUnit
        };
        return copy;
    }

    public override string Output()
    {
        string output = string.Empty;

        if (this.Tag == "rebalance_statblock")
        {
            output = this.FormatLineDropValue();
        }
        else if (this.Tag == "is_female")
        {
            output = this.FormatLineWithWhitespaceDropValue();
        }
        else
        {
            output = this.FormatLine();
        }

        if (this.EndOfUnit)
        {
            output += Format.UniversalNewLine();
        }

        return output;
    }


    private string FormatLine() => string.Format("{0}{1}{2}{3}", this.Tag, Format.GetWhiteSpace(this.Tag, 20, ' '), this.Value, Format.UniversalNewLine());

    private string FormatLineDropValue() => string.Format("{0} {1}", this.Tag, Format.UniversalNewLine());

    private string FormatLineWithWhitespaceDropValue() => string.Format("{0}{1}{2}", this.Tag, Format.GetWhiteSpace(this.Tag, 20, ' '), Format.UniversalNewLine());

}
