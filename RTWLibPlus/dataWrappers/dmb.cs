namespace RTWLibPlus.dataWrappers;
using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;

public class DMB : BaseWrapper, IWrapper
{
    private readonly string name = "dmb";
    public int DefaultNeeded { get; set; }
    public int NoDefaultNeeded { get; set; }

    public string GetName() => this.name;
    public DMB(string outputPath, string loadPath)
    {
        this.OutputPath = outputPath;
        this.LoadPath = loadPath;
    }

    public DMB(List<IBaseObj> data, TWConfig config)
    {
        this.Data = data;
        this.LoadPath = config.GetPath(Operation.Load, "dmb");
        this.OutputPath = config.GetPath(Operation.Save, "dmb");
        Console.WriteLine(this.OutputPath);
    }

    public void Parse() => this.Data = RFH.ParseFile(Creator.DMBcreator, ' ', false, this.LoadPath);

    public string Output()
    {
        string output = string.Empty;

        for (int i = 0; i < this.Data.Count; i++)
        {
            DMBObj obj = (DMBObj)this.Data[i];
            DMBObj next = new();
            if (i + 1 < this.Data.Count)
            {
                next = (DMBObj)this.Data[i + 1];
            }
            if (next.Ident == "type")
            {
                output += obj.Output() + Format.UniversalNewLine();
            }
            else
            {
                output += obj.Output();
            }
        }

        return output + Format.UniversalNewLine();
    }

    public void AddFallBacksForAllTypes()
    {
        Dictionary<int, int> chunks = this.GetChunkIndexes("type", "type");
        int modifier = 0;
        int placement = 0;
        foreach (KeyValuePair<int, int> pair in chunks)
        {
            bool hasDefault = false;
            IBaseObj insertion = null;
            IBaseObj type = this.Data[pair.Key + modifier];
            for (int i = pair.Key + modifier; i < pair.Key + pair.Value + modifier; i++)
            {
                string line = this.Data[i].Output();

                if (!line.StartsWith(';') && line.Contains("texture ") && line.Contains("Default "))
                {
                    hasDefault = true;
                }

                if (!line.StartsWith(';') && (line.Contains("texture ") || line.Contains("texture\t")) && !line.Contains("pbr_"))
                {
                    insertion = this.Data[i];
                    placement = i + 1;
                }

                if (hasDefault)
                {
                    this.NoDefaultNeeded += 1;
                    break;
                }
            }

            if (!hasDefault && insertion != null)
            {
                insertion = ChangeFaction(insertion);

                if (insertion != null)
                {
                    this.DefaultNeeded += 1;
                    this.Data.Insert(placement, insertion);
                    modifier += 1;
                }

                else
                {
                    this.NoDefaultNeeded += 1;
                }

                continue;
            }
        }

    }

    private static IBaseObj ChangeFaction(IBaseObj obj)
    {

        IBaseObj copy = obj.Copy();
        string[] split = copy.Value.Split(",");
        // handles cases where a tab is the delim
        string[] tagSplit = copy.Tag.Split("\t");

        if (split.Length == 1 && tagSplit.Length == 1)
        {
            return null;
        }

        if (tagSplit.Length > split.Length)
        {
            tagSplit[1] = "Default,";
            string val = tagSplit.ToString('\t');
            copy.Tag = val;
        }

        else
        {
            split[0] = "Default";
            string val = split.ToString(',');
            copy.Value = val;
        }

        return copy;
    }

    public void Clear() => this.Data.Clear();
}
