namespace RTWLibPlus.parsers.objects;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.configs.whiteSpace;

public class SMFObj : BaseObj, IBaseObj
{
    public SMFObj(string tag, string value, int depth) :
        base(tag, value, depth)
    {
        WSConfigFactory factory = new();
        this.WhiteSpaceConfig = factory.Create_DR_DS_SMF_WhiteSpace();
        this.Ident = this.Tag.Split(this.WhiteSpaceChar)[0];
    }

    public SMFObj() { }

    public override IBaseObj Copy()
    {
        SMFObj copy = new()
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

    public override string Output() => "Not Implemented";
}
