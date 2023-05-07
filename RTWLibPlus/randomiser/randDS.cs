using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.randomiser
{
    public static class RandDS
    {
        public static string RandCitiesBasic(DS ds = null)
        {
            TWRand.RefreshRndSeed();

            if (ds == null)
                ds = new DS(RFH.ParseFile(Creator.DScreator, ' ', false, "resources", "descr_strat.txt"));

            var settlements = ds.GetItemsByIdent("settlement").DeepCopy();
            ds.DeleteValue(ds.data, "settlement");
            var factions = TWRand.GetFactionListAndShuffle(0);

            int settlementsPerFaction = settlements.Count / factions.Length;

            while (settlements.Count > 0)
            {
                foreach (var faction in factions)
                {
                    if (settlements.Count == 0)
                        break;

                    int index;
                    ds.InsertNewObjectByCriteria(ds.data, settlements.GetRandom(out index, TWRand.rnd), string.Format("faction\t{0},", faction), "denari");
                    settlements.RemoveAt(index);
                }
            }


            return "Random city allocation complete";
        }

        /*public static string MatchCharacterCoordsToCities()
        {

        }*/




    }
}
