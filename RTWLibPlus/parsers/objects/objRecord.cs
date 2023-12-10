namespace RTWLibPlus.parsers.objects;

public struct ObjRecord
{
    public string Tag { get; set; }
    public string Ident { get; set; }
    public string Value { get; set; }
    public int Depth { get; set; }
    public int NewLinesAfter { get; set; }

    public ObjRecord(string tag, string ident, string value, int depth, int newLinesAfter = 0)
    {
        this.Ident = ident;
        this.Tag = tag;
        this.Value = value;
        this.Depth = depth;
        this.NewLinesAfter = newLinesAfter;
    }

}
