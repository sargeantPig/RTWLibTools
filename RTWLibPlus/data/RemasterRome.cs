using RTWLibPlus.helpers;
using RTWLibPlus.parsers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace RTWLibPlus.data
{
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
            DepthParse dp = new DepthParse();
            string json = dp.ReadFileAsString(path);
            return JsonSerializer.Deserialize<TWConfig>(json);
        }

        public string GetPath(Operation state, string file)
        {
            if (state == Operation.Save)
                return RFH.CurrDirPath(BaseFolder, Save, Paths[file]);
            else return RFH.CurrDirPath(BaseFolder, Load, Paths[file]);
        }
    }
}
