using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.map;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RTWLibPlus.randomiser
{
    public static class RandDS
    {
        public static string RandCitiesBasic(DS ds = null, CityMap cm = null)
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

            MatchCharacterCoordsToCities(ds, cm);

            return "Random city allocation complete";
        }

        private static void MatchCharacterCoordsToCities(DS ds, CityMap cm)
        {
            TWRand.RefreshRndSeed();
            var factions = TWRand.GetFactionListAndShuffle(0);

            foreach(var f in factions)
            {
                var settlements = ds.GetItemsByCriteria("character", "settlement", string.Format("faction\t{0},", f));
                var regions = ds.GetItemsByCriteriaDepth(settlements, "", "region", "settlement");
                var characters = ds.GetItemsByCriteria("character_record", "character", string.Format("faction\t{0},", f));
                ChangeCharacterCoords(regions, characters, cm);
            }

        }

        private static void ChangeCharacterCoords(List<IbaseObj> regions, List<IbaseObj> characters, CityMap cm)
        {
            int ri = 0;
            foreach(baseObj c in characters)
            {
                if (ri >= regions.Count)
                    ri = 0;

                int[] coord = cm.CityCoordinates[((baseObj)regions[ri]).Value];
                c.Value = DS.ChangeCharacterCoordinates(c.Value, coord);
                ri++;
            }


        }

        


    }
}
