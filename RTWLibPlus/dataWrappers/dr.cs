namespace RTWLibPlus.dataWrappers;

using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System.Collections.Generic;

public class DR : BaseWrapper, IWrapper
{
    private readonly string name = "dr";

    public string GetName() => this.name;

    private readonly Dictionary<string, string> regionsByColour = new();
    public List<string> Regions { get; set; } = new();

    public DR(string outputPath, string loadPath)
    {
        this.OutputPath = outputPath;
        this.LoadPath = loadPath;
    }

    public DR(List<IBaseObj> data, TWConfig config)
    {
        this.Data = data;
        this.GetRegionsByColour();
        this.OutputPath = config.GetPath(Operation.Save, "dr");
        this.LoadPath = config.GetPath(Operation.Load, "dr");
    }

    public void Clear()
    {
        this.Data.Clear();
        this.regionsByColour.Clear();
    }

    public void Parse()
    {

        this.Data = RFH.ParseFile(Creator.DRcreator, '\t', false, this.LoadPath);
        this.GetRegionsByColour();
    }

    public string GetRegionByColour(int r, int g, int b)
    {
        string key = string.Format("{0} {1} {2}", r, g, b);
        string region = this.regionsByColour[key];
        return region;
    }

    public string Output()
    {
        string output = string.Empty;
        foreach (DRObj obj in this.Data)
        {
            //output += obj.Output();
        }
        RFH.Write(this.OutputPath, output);

        return output;
    }

    private void GetRegionsByColour()
    {
        int pos = 0;
        for (int i = 0; i < this.Data.Count + 1; i++)
        {
            if (pos == 8)
            {
                this.regionsByColour.Add(this.Data[i - 4].Value, this.Data[i - pos].Value);
                this.Regions.Add(this.Data[i - pos].Value);
                pos = 0;
            }
            pos++;
        }
    }
}
