namespace RTWLibPlus.dataWrappers;

using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System.Collections.Generic;

public class EDB : BaseWrapper, IWrapper
{
    private readonly string name = "edb";

    public string GetName() => this.name;

    public EDB(string outputPath, string loadPath)
    {
        this.OutputPath = outputPath;
        this.LoadPath = loadPath;
    }

    public EDB(List<IBaseObj> data, TWConfig config)
    {
        this.Data = data;
        this.Sanitise(data);
        this.LoadPath = config.GetPath(Operation.Load, "edb");
        this.OutputPath = config.GetPath(Operation.Save, "edb");
    }
    public void Parse()
    {
        this.Data = RFH.ParseFile(Creator.EDBcreator, ' ', false, this.LoadPath);
        this.Sanitise(this.Data);
    }

    public void Clear() => this.Data.Clear();

    private void Sanitise(List<IBaseObj> toSanitise)
    {
        foreach (BaseObj obj in toSanitise)
        {
            if (obj.Tag == "building" || obj.Value == "building")
            {
                this.Swap(obj);
            }
        }
    }

    private void Swap(BaseObj obj) => (obj.Value, obj.Tag) = (obj.Tag, obj.Value);

    public string Output()
    {
        this.Sanitise(this.Data);
        string output = string.Empty;
        foreach (EDBObj obj in this.Data)
        {
            output += obj.Output();
        }
        //RFH.Write(this.OutputPath, output);
        return output;
    }
}
