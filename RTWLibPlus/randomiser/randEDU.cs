using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
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

        public static string RandomiseOwnership(EDU edu, int listToFetch = 0, int maxPerUnit = 3, int minimumPerUnit = 1)
        {
            TWRand.RefreshRndSeed();

            if(edu == null)
                edu = new EDU(RFH.ParseFile(Creator.EDUcreator, ' ', false, "resources", "export_descr_unit.txt"));

            var ownerships = edu.GetItemsByIdent("ownership");
            List<string> factionList = TWRand.GetFactionListAndShuffle(listToFetch).ToList();

            
            foreach ( EDUObj ownership in ownerships ) {

                if (factionList.Count() < maxPerUnit)
                    factionList = TWRand.GetFactionListAndShuffle(listToFetch).ToList();

                string[] newFactions = new string[maxPerUnit + 1] ;
                for(int i = 0; i < maxPerUnit; i++)
                {
                    int index;
                    newFactions[i] = factionList.GetRandom(out index, TWRand.rnd);
                    factionList.RemoveAt(index);
                }
                newFactions[newFactions.Count() - 1] = "slave";

                ownership.Value = newFactions.ToString(',', ' ');
            }

            AddAttributeAll(edu, "mercenary_unit");
            
            return "Random ownership complete";
        }

    }
}
