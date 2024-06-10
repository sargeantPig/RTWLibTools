namespace RTWLibPlus.dataWrappers;

using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;

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

    public void AddUnitToArmy(IBaseObj faction, IBaseObj character, IBaseObj unit)
    {
        bool add = this.InsertNewObjectByCriteria(this.Data, unit, faction.Tag, character.Tag, "unit");
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

    public static string GetUnitName(IBaseObj unit)
    {
        string name = string.Format("{0} {1}", unit.Tag.Split('\t', StringSplitOptions.RemoveEmptyEntries)[1], unit.Value.GetFirstWord('\t'));
        return name;
    }
}
