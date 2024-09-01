namespace RTWLibPlus.dataWrappers;
using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Linq;

public class EDU : BaseWrapper, IWrapper
{
    private readonly string name = "edu";

    public string GetName() => this.name;
    public EDU(string outputPath, string loadPath)
    {
        this.OutputPath = outputPath;
        this.LoadPath = loadPath;
    }

    public EDU(List<IBaseObj> data, TWConfig config)
    {
        this.Data = data;
        this.SetEndOfUnits();
        //this.PrepareEDU();
        this.LoadPath = config.GetPath(Operation.Load, "edu");
        this.OutputPath = config.GetPath(Operation.Save, "edu");
    }

    public void Parse()
    {
        this.Data = RFH.ParseFile(Creator.EDUcreator, ' ', false, this.LoadPath);
        this.SetEndOfUnits();
    }

    public string Output()
    {
        string output = string.Empty;

        foreach (EDUObj obj in this.Data)
        {
            output += obj.Output();
        }

        //RFH.Write(this.OutputPath, output + Format.UniversalNewLine());
        return output + Format.UniversalNewLine();
    }


    public void PrepareEDU() => this.DeleteChunks("type", "rebalance_statblock");

    public void Clear() => this.Data.Clear();

    private void SetEndOfUnits()
    {
        for (int i = 0; i < this.Data.Count; i++)
        {
            if (i + 1 >= this.Data.Count)
            {
                return;
            }

            EDUObj obj = (EDUObj)this.Data[i];
            EDUObj nextObj = (EDUObj)this.Data[i + 1];
            if (nextObj.Tag is "type" or "rebalance_statblock" or "is_female")
            {

                obj.EndOfUnit = true;
            }
        }
    }

    public List<string> GetUnitsFromFaction(string faction, string[] filterOut, string entryKey = "type")
    {
        List<IBaseObj> ownerships = this.GetItemsByIdent("ownership");
        List<IBaseObj> type = this.GetItemsByIdent(entryKey);

        List<string> units = [];

        for (int i = 0; i < ownerships.Count; i++)
        {
            IBaseObj obj = ownerships[i];
            IBaseObj unit = type[i];

            bool isFiltered = filterOut.Any(sub => unit.Tag.Contains(sub) || unit.Value.Contains(sub));

            if (isFiltered)
            {
                continue;
            }

            if (obj.Value.Contains(faction))
            {
                units.Add(unit.Value);
            }
        }
        return units;
    }

    public void RemoveAttributesAll(params string[] attriToRemove)
    {
        List<IBaseObj> attri = this.GetItemsByIdent("attributes");

        foreach (EDUObj a in attri)
        {
            string[] values = a.Value.Split(',').TrimAll();
            string[] newVals = [];
            foreach (string val in values)
            {
                if (!attriToRemove.Contains(val))
                {
                    newVals = newVals.Add(val);
                }
            }

            a.Value = newVals.ToString(',', ' ');
        }
    }
}
