namespace RTWLibPlus.randomiser;
using RTWLibPlus.data.unit;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;

public static class RandEDU
{
    public static string AddAttributeAll(EDU edu, string attribute)
    {
        List<IBaseObj> attributes = edu.GetItemsByIdent("attributes");

        foreach (EDUObj attri in attributes)
        {
            if (!attri.Value.Contains(attribute))
            {
                attri.Value += ", " + attribute;
            }
        }

        return string.Format("Added {0} to all units", attribute);

    }
    public static string RandomiseOwnership(EDU edu, RandWrap rnd, SMF smf, int maxPerUnit = 3, int minimumPerUnit = 1)
    {
        rnd.RefreshRndSeed();

        List<IBaseObj> attributes = edu.GetItemsByIdent("attributes");
        List<IBaseObj> ownerships = edu.GetItemsByIdent("ownership");
        List<IBaseObj> category = edu.GetItemsByIdent("category");
        List<string> factionList = smf.GetFactions();
        factionList.Shuffle(rnd.RND);

        for (int io = 0; io < ownerships.Count; io++)
        {
            EDUObj ownership = (EDUObj)ownerships[io];
            if (factionList.Count < maxPerUnit)
            {
                factionList = smf.GetFactions();
                factionList.Shuffle(rnd.RND);
            }

            if (attributes[io].Value.Contains("general") || category[io].Value.Contains("ship"))
            {
                continue;
            }

            string[] newFactions = new string[maxPerUnit];
            for (int i = 0; i < maxPerUnit; i++)
            {
                newFactions[i] = factionList.GetRandom(out int index, rnd.RND);
                factionList.RemoveAt(index);
            }

            ownership.Value = newFactions.ToString(',', ' ') + ", slave";
        }

        AddAttributeAll(edu, "mercenary_unit");
        //SetGeneralUnits(edu, smf, rnd, 700, 950);
        return "Random ownership complete";
    }
    public static string SetGeneralUnits(EDU edu, SMF smf, RandWrap rnd, int minPriceEarly, int minPriceLate)
    {
        Dictionary<string, bool[]> fHasGenerals = smf.GetFactions().ToArray().InitDictFromList(new bool[2] { false, false });
        List<IBaseObj> ownerships = edu.GetItemsByIdent("ownership");
        List<IBaseObj> costs = edu.GetItemsByIdent("stat_cost");
        edu.RemoveAttributesAll("general_unit", "general_unit_upgrade \"marian_reforms\"");
        List<IBaseObj> attr = edu.GetItemsByIdent("attributes");

        List<string> factions = smf.GetFactions();

        UnitsWrapper uw = new(edu);

        foreach (string f in factions)
        {
            List<int> unitOwned = new();
            List<int> shortlist = new();

            for (int i = 0; i < ownerships.Count; i++)
            {
                if (ownerships[i].Value.Contains(f))
                {
                    unitOwned.Add(i);
                }
            }

            foreach (int unit in unitOwned)
            {
                string[] cost = costs[unit].Value.Split(',').TrimAll();
                string[] attributes = attr[unit].Value.Split(',').TrimAll();

            }

        }

        // get units of a faction

        // shortlist the most expensive/best stats

        // pick from shortlist

        // general random bottom half

        // marian reform general random top half


        new List<IBaseObj>[3] { ownerships, attr, costs }.ShuffleMany(rnd.RND);

        for (int i = 0; i < ownerships.Count; i++)
        {
            EDUObj a = (EDUObj)attr[i];
            EDUObj b = (EDUObj)ownerships[i];
            EDUObj cost = (EDUObj)costs[i];

            string[] asplit = a.Value.Split(',').TrimAll();
            string[] bsplit = b.Value.Split(',').TrimAll();
            string[] costStr = cost.Value.Split(",").TrimAll();

            if (Convert.ToInt16(costStr[1]) >= minPriceEarly && Convert.ToInt16(costStr[1]) < minPriceLate)
            {
                ((EDUObj)attr[i]).Value = asplit.Add("general_unit").ToString(',', ' ');

                foreach (string faction in bsplit)
                {
                    if (fHasGenerals.ContainsKey(faction))
                    {
                        fHasGenerals[faction][0] = true;
                    }
                }

            }
            if (Convert.ToInt16(costStr[1]) >= minPriceLate)
            {
                asplit = asplit.Add("general_unit_upgrade \"marian_reforms\"");
                asplit = asplit.FindAndRemove("general_unit");
                ((EDUObj)attr[i]).Value = asplit.ToString(',', ' ');
                foreach (string faction in bsplit)
                {
                    if (fHasGenerals.ContainsKey(faction))
                    {
                        fHasGenerals[faction][1] = true;
                    }
                }
            }
            else
            {
                continue;
            }
        }

        return "Generals set";
    }
}
