using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.randomiser;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace RTWLibPlus.dataWrappers
{
    public class DS : BaseWrapper, IWrapper
    {
        private readonly string name = "ds";

        public string GetName()
        {
            return name;
        }

        public DS(string outputPath, string loadPath)
        {
            OutputPath = outputPath;
            LoadPath = loadPath;
        }

        public DS(List<IbaseObj> data, TWConfig config)
        {
            this.data = data;
            SetLastOfGroup();
            LoadPath = config.GetPath(Operation.Load, "ds");
            OutputPath = config.GetPath(Operation.Save, "ds");
        }

        public void Parse()
        {
            this.data = RFH.ParseFile(Creator.DScreator, ' ', false, LoadPath);
            SetLastOfGroup();
        }


        public string Output()
        {
            string output = string.Empty;
            foreach (DSObj obj in data)
            {
                  output += obj.Output();
            }
            RFH.Write(OutputPath, output);
            return output;
        }

        public void Clear()
        {
            data.Clear();
        }

        private void SetLastOfGroup()
        {
            string previous = string.Empty;
            foreach (DSObj obj in data)
            {
                string ident = obj.Tag.Split('\t')[0];

                if (ident != previous)
                    obj.lastOfGroup = true;

                previous = ident;
            }
        }

        public static string ChangeCharacterCoordinates(string character, Vector2 coords)
        {
            string[] split = character.Split(',').TrimAll();
            string x = string.Format("x {0}", (int)coords.X);
            string y = string.Format("y {0}", (int)coords.Y);
            split[split.Length - 1] = y;
            split[split.Length - 2] = x;
            return split.ToString(',', ' ');
        }

        public Dictionary<string, List<IbaseObj>> GetSettlementsByFaction(SMF smf)
        {
            Dictionary<string, List<IbaseObj>> settlementsByFaction = new Dictionary<string, List<IbaseObj>>();
            foreach (var f in smf.GetFactions())
            {
                var settlements = GetItemsByCriteria("character", "settlement", string.Format("faction\t{0},", f));
                settlementsByFaction.Add(f, settlements);
            }

            return settlementsByFaction;
        }

        public string GetFactionByRegion(string region)
        {
            var s = GetTagByContentsValue(data, "faction", region);

            if (s == null)
                return "slave";

            return s.Split('\t')[1].Trim(',');
        }
    }
}
