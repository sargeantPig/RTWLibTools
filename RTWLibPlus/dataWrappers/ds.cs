﻿using RTWLibPlus.data;
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
using System.Text;

namespace RTWLibPlus.dataWrappers
{
    public class DS : BaseWrapper, IWrapper
    {
        public DS()
        {
            LoadPath = RemasterRome.GetPath(false, "ds");
            OutputPath = RemasterRome.GetPath(true, "ds");
        }

        public DS(List<IbaseObj> data)
        {
            this.data = data;
            SetLastOfGroup();
            LoadPath = RemasterRome.GetPath(false, "ds");
            OutputPath = RemasterRome.GetPath(true, "ds");
        }

        public void Parse(string path = "")
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
                string ident = obj.Tag.Split(DSObj.whiteSpace)[0];

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

        public Dictionary<string, List<IbaseObj>> GetSettlementsByFaction()
        {
            Dictionary<string, List<IbaseObj>> settlementsByFaction = new Dictionary<string, List<IbaseObj>>();
            foreach (var f in TWRand.GetFactionListAndShuffle(0))
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
