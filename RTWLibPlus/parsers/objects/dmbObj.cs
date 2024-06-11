namespace RTWLibPlus.parsers.objects;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.configs.whiteSpace;

public class DMBObj : BaseObj, IBaseObj
{
    public DMBObj(string tag, string value, int depth) :
        base(tag, value, depth)
    {
        WSConfigFactory factory = new();
        this.WhiteSpaceConfig = WSConfigFactory.Create_DR_DS_SMF_WhiteSpace();
        this.Ident = this.Tag.Split(this.WhiteSpaceChar)[0];
    }

    public DMBObj() { }

    public override IBaseObj Copy()
    {
        DMBObj copy = new()
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
        string output = "";

        if (this.Tag == this.Value)
        {
            this.Value = "";
        }

        output = string.Format("{0} {1}", this.Tag, this.Value);

        return output + Format.UniversalNewLine();
    }

}
