using RTWLibPlus.data;
using RTWLibPlus.data.unit;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTWLibPlus.randomiser
{
    public static class RandEDU
    {
        public static string AddAttributeAll(EDU edu, string attribute) {
            var attributes = edu.GetItemsByIdent("attributes");

            foreach (EDUObj attri in attributes)
            {
                if (!attri.Value.Contains(attribute))
                    attri.Value += ", " + attribute;
            }

            return string.Format("Added {0} to all units", attribute);

        }
        public static string RandomiseOwnership(EDU edu, RandWrap rnd, SMF smf, int maxPerUnit = 3, int minimumPerUnit = 1)
        {
            rnd.RefreshRndSeed();

         
            var ownerships = edu.GetItemsByIdent("ownership");
            List<string> factionList = smf.GetFactions();
            factionList.Shuffle(rnd.RND);

            
            foreach ( EDUObj ownership in ownerships ) {

                if (factionList.Count() < maxPerUnit)
                {
                    factionList = smf.GetFactions();
                    factionList.Shuffle(rnd.RND);
                }

                string[] newFactions = new string[maxPerUnit + 1] ;
                for(int i = 0; i < maxPerUnit; i++)
                {
                    int index;
                    newFactions[i] = factionList.GetRandom(out index, rnd.RND);
                    factionList.RemoveAt(index);
                }
                newFactions[newFactions.Count() - 1] = "slave";

                ownership.Value = newFactions.ToString(',', ' ');
            }

            AddAttributeAll(edu, "mercenary_unit");
            SetGeneralUnits(edu, smf, rnd, 700, 950);
            return "Random ownership complete";
        }
        public static string SetGeneralUnits(EDU edu, SMF smf, RandWrap rnd, int minPriceEarly, int minPriceLate)
        {
            var FHasGenerals =  smf.GetFactions().ToArray().InitDictFromList(new bool[2] { false, false});
            var ownerships = edu.GetItemsByIdent("ownership");
            var costs = edu.GetItemsByIdent("stat_cost");
            edu.RemoveAttributesAll("general_unit", "general_unit_upgrade \"marian_reforms\"");
            var attr = edu.GetItemsByIdent("attributes");

            var factions = smf.GetFactions();

            UnitsWrapper uw = new UnitsWrapper(edu);

            foreach (var f in factions)
            {
                List<int> unitOwned = new List<int>();
                List<int> shortlist = new List<int>();

                for (int i = 0; i < ownerships.Count; i++)
                {
                    if (ownerships[i].Value.Contains(f))
                        unitOwned.Add(i);
                }

                foreach(var unit in unitOwned)
                {
                    string[] cost = costs[unit].Value.Split(',').TrimAll();
                    var attributes = attr[unit].Value.Split(',').TrimAll();

                }

            }

            // get units of a faction

            // shortlist the most expensive/best stats 

            // pick from shortlist

            // general random bottom half

            // marian reform general random top half


            new List<IbaseObj>[3] { ownerships, attr, costs }.ShuffleMany(rnd.RND);  

            for (int i = 0; i < ownerships.Count; i++)
            {
                EDUObj a = (EDUObj)attr[i];
                EDUObj b = (EDUObj)ownerships[i];
                EDUObj cost = (EDUObj)costs[i];

                string[] asplit = a.Value.Split(',').TrimAll();
                string[] bsplit = b.Value.Split(',').TrimAll();
                string[] costStr = cost.Value.Split(",").TrimAll();

                bool cont = false;


                if (Convert.ToInt16(costStr[1]) >= minPriceEarly && Convert.ToInt16(costStr[1]) < minPriceLate)
                {
                    ((EDUObj)attr[i]).Value = asplit.Add("general_unit").ToString(',', ' ');

                    foreach (var faction in bsplit)
                    {
                        if (FHasGenerals.ContainsKey(faction))
                            FHasGenerals[faction][0] = true;
                    }

                }
                if (Convert.ToInt16(costStr[1]) >= minPriceLate)
                {
                    asplit = asplit.Add("general_unit_upgrade \"marian_reforms\"");
                    asplit = asplit.FindAndRemove("general_unit");
                    ((EDUObj)attr[i]).Value = asplit.ToString(',', ' ');
                    foreach (var faction in bsplit)
                    {
                        if (FHasGenerals.ContainsKey(faction))
                            FHasGenerals[faction][1] = true;
                    }
                }
                else continue;

                
            }

            return "Generals set";
        }
    }
}
