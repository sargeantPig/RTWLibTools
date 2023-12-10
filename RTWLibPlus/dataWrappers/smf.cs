namespace RTWLibPlus.dataWrappers;
using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SMF : BaseWrapper, IWrapper
{
    private readonly string name = "smf";
    private List<string> factions;

    public string GetName() => this.name;
    public SMF(string outputPath, string loadPath)
    {
        this.OutputPath = outputPath;
        this.LoadPath = loadPath;

    }


    public SMF(List<IBaseObj> data, TWConfig config)
    {
        this.Data = data;
        this.Sanitise(data);
        this.LoadPath = config.GetPath(Operation.Load, "smf");
        this.OutputPath = config.GetPath(Operation.Save, "smf");
        this.ExtractFactions();
    }
    public void Parse()
    {
        this.Data = RFH.ParseFile(Creator.SMFcreator, ':', false, this.LoadPath);
        this.Sanitise(this.Data);
        this.ExtractFactions();

    }
    public string Output()
    {
        string output = string.Empty;
        foreach (BaseObj obj in this.Data)
        {
            //output += obj.Output();
        }
        RFH.Write(this.OutputPath, output);
        return output;
    }

    public void Clear() => this.Data.Clear();

    public List<string> GetFactions()
    {
        if (this.factions.Count == 0)
        {
            this.ExtractFactions();
        }

        return new List<string>(this.factions);
    }

    private void ExtractFactions()
    {
        List<string> fs = new();
        foreach (BaseObj obj in this.Data)
        {
            if (obj != null && obj.Ident != "]" && obj.Ident != "[" && obj.Ident != "factions")
            {
                fs.Add(obj.Ident);
            }
        }
        this.factions = new List<string>(fs);

    }

    private void Sanitise(List<IBaseObj> toSanitise) => Parallel.ForEach(toSanitise, obj =>
                                                             {
                                                                 obj.Value = obj.Value.Trim();
                                                                 obj.Tag = obj.Tag.Trim();
                                                                 obj.Ident = obj.Ident.Trim();
                                                                 obj.Value = obj.Value.Trim(',');
                                                                 obj.Value = obj.Value.Trim(':');
                                                                 obj.Value = obj.Value.Trim('"');
                                                                 obj.Tag = obj.Tag.Trim(',');
                                                                 obj.Tag = obj.Tag.Trim(':');
                                                                 obj.Tag = obj.Tag.Trim('"');
                                                                 obj.Ident = obj.Ident.Trim(',');
                                                                 obj.Ident = obj.Ident.Trim(':');
                                                                 obj.Ident = obj.Ident.Trim('"');
                                                                 this.Sanitise(obj.GetItems());
                                                             });
}
