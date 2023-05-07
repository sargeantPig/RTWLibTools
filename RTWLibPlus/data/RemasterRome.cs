using RTWLibPlus.helpers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RTWLibPlus.data
{
    public static class RemasterRome
    {
        public static string[] factions = {
            "egypt",
            "seleucid",
            "carthage",
            "parthia",
            "gauls",
            "germans",
            "britons",
            "greek_cities",
            "macedon",
            "pontus",
            "armenia",
            "dacia",
            "numidia",
            "scythia",
            "spain",
            "thrace",
            "romans_julii",
            "romans_brutii",
            "romans_scipii",
            "romans_senate"
        };

        public static string[] factionsRomanCombined = {
            "egypt",
            "seleucid",
            "carthage",
            "parthia",
            "gauls",
            "germans",
            "britons",
            "greek_cities",
            "macedon",
            "pontus",
            "armenia",
            "dacia",
            "numidia",
            "scythia",
            "spain",
            "thrace",
            "roman"
        };

        public static string[] factionCultures = {
            "roman", "greek", "carthaginian", "eastern", "egyptian", "barbarian"
        };

        public static string baseFolder = @"Mods\My Mods\randomiser";
        public static string load = "vanilla";
        public static string save = "data";
        public static Dictionary<string, string> paths = new Dictionary<string, string>()
        {
            { "edu", @"export_descr_unit.txt"},
            { "ds", @"world\maps\campaign\imperial_campaign\descr_strat.txt"}

        };

        public static string GetPath(bool state, string file)
        {
            if (state)
                return RFH.CurrDirPath(baseFolder, save, paths[file]);
            else return RFH.CurrDirPath(baseFolder, load, paths[file]);
        }

        public static string[] GetFactionList(int listToFetch)
        {
            switch(listToFetch)
            {
                case 0: return factions;
                case 1: return factionsRomanCombined;
                case 2: return factionCultures;
                    default: return factions;
            }
        }
    }
}
