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

    public class RemasterRome
    {
        public string[] Factions { get;  set; }
        public string[] FactionsRomanCombined { get; set; }
        public string[] FactionCultures { get; set; }

        public string BaseFolder { get; set; }
        public string Load { get; set; }
        public string Save { get; set; }

        public static RemasterRome LoadConfig(string path)
        {
            DepthParse dp = new DepthParse();
            string json = dp.ReadFileAsString(path);
            return JsonSerializer.Deserialize<RemasterRome>(json);
        }

        public Dictionary<string, string> Paths { get; set; }

        public string GetPath(Operation state, string file)
        {
            if (state == Operation.Save)
                return RFH.CurrDirPath(BaseFolder, Save, Paths[file]);
            else return RFH.CurrDirPath(BaseFolder, Load, Paths[file]);
        }

        public string[] GetFactionList(int listToFetch)
        {
            switch(listToFetch)
            {
                case 0: return Factions;
                case 1: return FactionsRomanCombined;
                case 2: return FactionCultures;
                default: return Factions;
            }
        }

    }
}
