namespace RTWLibPlus.parsers.objects;
using static RTWLibPlus.parsers.DepthParse;

public static class Creator
{
    public static readonly ObjectCreator EDUcreator = (value, tag, depth) => new EDUObj(tag, value, depth);
    public static readonly ObjectCreator DRcreator = (value, tag, depth) => new DRObj(tag, value, depth);
    public static readonly ObjectCreator DScreator = (value, tag, depth) => new DSObj(tag, value, depth);
    public static readonly ObjectCreator EDBcreator = (value, tag, depth) => new EDBObj(tag, value, depth);
    public static readonly ObjectCreator SMFcreator = (value, tag, depth) => new SMFObj(tag, value, depth);
}
