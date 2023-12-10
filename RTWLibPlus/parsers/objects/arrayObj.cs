namespace RTWLibPlus.parsers.objects;
using RTWLibPlus.helpers;

public abstract class ArrayObj : BaseObj
{
    public ArrayObj() { }
    public ArrayObj(string tag, string value, int depth) : base(tag, value, depth)
    {

    }

    public string CloseBrackets() => string.Format("{0}}}{1}",
            Format.GetWhiteSpace("", this.WhiteSpaceMultiplier * this.Depth, this.WhiteSpaceChar),
            Format.UniversalNewLine());

    public string OpenBrackets() => string.Format("{0}{{{1}",
            Format.GetWhiteSpace("", this.WhiteSpaceMultiplier * this.Depth, this.WhiteSpaceChar),
            Format.UniversalNewLine());

}
