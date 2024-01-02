﻿namespace RTWLibPlus.dataWrappers;

using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System.Collections.Generic;
using System.Numerics;

public class DS : BaseWrapper, IWrapper
{
    private readonly string name = "ds";

    public string GetName() => this.name;

    public DS(string outputPath, string loadPath)
    {
        this.OutputPath = outputPath;
        this.LoadPath = loadPath;
    }

    public DS(List<IBaseObj> data, TWConfig config)
    {
        this.Data = data;
        this.SetLastOfGroup();
        this.LoadPath = config.GetPath(Operation.Load, "ds");
        this.OutputPath = config.GetPath(Operation.Save, "ds");
    }

    public void Parse()
    {
        this.Data = RFH.ParseFile(Creator.DScreator, ' ', false, this.LoadPath);
        this.SetLastOfGroup();
    }


    public string Output()
    {
        string output = string.Empty;
        foreach (DSObj obj in this.Data)
        {
            output += obj.Output();
        }
        RFH.Write(this.OutputPath, output);
        return output;
    }

    public void Clear() => this.Data.Clear();

    private void SetLastOfGroup()
    {
        string previous = string.Empty;
        foreach (DSObj obj in this.Data)
        {
            string ident = obj.Tag.Split('\t')[0];

            if (ident != previous)
            {
                obj.LastOfGroup = true;
            }

            previous = ident;
        }
    }

    public static string ChangeCharacterCoordinates(string character, Vector2 coords, Vector2 closeWater)
    {
        string[] split = character.Split(',').TrimAll();

        if (split[1].Contains("admiral"))
        {
            coords = closeWater;
        }

        string x = string.Format("x {0}", (int)coords.X);
        string y = string.Format("y {0}", (int)coords.Y);
        split[^1] = y;
        split[^2] = x;
        return split.ToString(',', ' ');
    }

    public Dictionary<string, List<IBaseObj>> GetSettlementsByFaction(SMF smf)
    {
        Dictionary<string, List<IBaseObj>> settlementsByFaction = new();
        foreach (string f in smf.GetFactions())
        {
            List<IBaseObj> settlements = this.GetItemsByCriteria("character", "settlement", string.Format("faction\t{0},", f));
            settlementsByFaction.Add(f, settlements);
        }

        return settlementsByFaction;
    }

    public string GetFactionByRegion(string region)
    {
        string s = this.GetTagByContentsValue(this.Data, "faction", region);

        if (s == null)
        {
            return "slave";
        }

        return s.Split('\t')[1].Trim(',');
    }


}
