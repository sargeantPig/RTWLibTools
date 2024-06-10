namespace RTWLibPlus.dataWrappers;
using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.IO;

public class DMB : BaseWrapper, IWrapper
{
    private readonly string name = "dmb";

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

        // if (Directory.Exists(this.OutputPath))
        // {
        //     RFH.Write(this.OutputPath, output + Format.UniversalNewLine());
        // }

        return output + Format.UniversalNewLine();
    }

    public void AddFallBacksForAllTypes()
    {
        Dictionary<int, int> chunks = this.GetChunkIndexes("type", "type");
        int modifier = 0;
        int errors = 0;
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
                }

                if (hasDefault)
                {
                    break;
                }
            }

            if (!hasDefault && insertion != null)
            {
                insertion = ChangeFaction(insertion);

                if (insertion != null)
                {
                    this.Data.Insert(pair.Key + pair.Value + modifier, insertion);
                    modifier += 1;
                }

                continue;
            }

            else if (insertion == null)
            {
                errors += 1;
                string str = this.Data[pair.Key + modifier].Value;
                Console.WriteLine("Error no texture for: " + str + "\n");
            }

            else
            {
                string str = this.Data[pair.Key + modifier].Value;
                Console.WriteLine("Error no texture for: " + str + "\n");
            }
        }

    }

    private static IBaseObj ChangeFaction(IBaseObj obj)
    {

        IBaseObj copy = obj.Copy();
        string[] split = copy.Value.Split(",");

        if (split.Length == 1)
        {
            return null;
        }

        split[0] = "Default";
        string val = split.ToString(',');
        copy.Value = val;

        return copy;
    }

    public void Clear() => this.Data.Clear();
}
