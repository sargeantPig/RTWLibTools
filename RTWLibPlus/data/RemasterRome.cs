namespace RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers;
using System.Collections.Generic;
using System.Text.Json;

public enum Operation
{
    Save,
    Load
}

public class TWConfig
{
    public Dictionary<string, string> Paths { get; set; }
    public string BaseFolder { get; set; }
    public string Load { get; set; }
    public string Save { get; set; }

    public static TWConfig LoadConfig(string path)
    {
        DepthParse dp = new();
        string json = dp.ReadFileAsString(path);
        return JsonSerializer.Deserialize<TWConfig>(json);
    }

    public string GetPath(Operation state, string file)
    {
        if (state == Operation.Save)
        {
            return RFH.CurrDirPath(this.BaseFolder, this.Save, this.Paths[file]);
        }
        else
        {
            return RFH.CurrDirPath(this.BaseFolder, this.Load, this.Paths[file]);
        }
    }
}
