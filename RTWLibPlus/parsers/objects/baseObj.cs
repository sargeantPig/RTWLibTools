namespace RTWLibPlus.parsers.objects;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.configs.whiteSpace;
using System.Collections.Generic;

public abstract class BaseObj : IBaseObj
{
    private WhiteSpaceConfig whiteSpaceConfig;
    private ObjRecord record;

    public BaseObj() { }

    public BaseObj(string tag, string value, int depth) => this.record = new ObjRecord(tag, tag.Split(this.WhiteSpaceConfig.WhiteChar)[0], value, depth);

    public abstract IBaseObj Copy();
    public abstract string Output();
    public List<IBaseObj> GetItems() => this.Items;
    public string NormalFormat(char whiteSpace, int end) => string.Format("{0}{1} {2}{3}",
            Format.GetWhiteSpace("", end, whiteSpace),
            this.record.Tag, this.record.Value,
            Format.UniversalNewLine());
    public string IgnoreValue(char whiteSpace, int end) => string.Format("{0}{1}{2}",
            Format.GetWhiteSpace("", end, whiteSpace),
            this.record.Tag,
            Format.UniversalNewLine());

    public string Tag
    {
        get => this.record.Tag;
        set => this.record.Tag = value;
    }
    public string Ident
    {
        get => this.record.Ident;
        set => this.record.Ident = value;
    }
    public string Value
    {
        get => this.record.Value;
        set => this.record.Value = value;
    }
    public int NewLinesAfter
    {
        get => this.record.NewLinesAfter;
        set => this.record.NewLinesAfter = value;
    }
    public int Depth
    {
        get => this.record.Depth;
        set => this.record.Depth = value;
    }
    public WhiteSpaceConfig WhiteSpaceConfig
    {
        get => this.whiteSpaceConfig;
        set => this.whiteSpaceConfig = value;
    }

    public char WhiteSpaceChar
    {
        get => this.whiteSpaceConfig.WhiteChar;
        set => this.whiteSpaceConfig.WhiteChar = value;
    }

    public int WhiteSpaceMultiplier
    {
        get => this.whiteSpaceConfig.WhiteDepthMultiplier;
        set => this.whiteSpaceConfig.WhiteDepthMultiplier = value;
    }

    public List<IBaseObj> Items { get; set; } = new();


}
