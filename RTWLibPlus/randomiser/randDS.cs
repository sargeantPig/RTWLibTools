using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.map;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace RTWLibPlus.randomiser
{
    public static class RandDS
    {
        public static string RandCitiesBasic(SMF smf, RandWrap rnd, DS ds = null, CityMap cm = null)
        {
            rnd.RefreshRndSeed();

            var settlements = ds.GetItemsByIdent("settlement").DeepCopy();
            ds.DeleteValue(ds.data, "settlement");
            List<string> factionList = smf.GetFactions();

            factionList.Shuffle(rnd.RND);
            string[] factions = factionList.ToArray();
            int settlementsPerFaction = settlements.Count / factions.Length;

            while (settlements.Count > 0)
            {
                foreach (var faction in factions)
                {
                    if (settlements.Count == 0)
                        break;

                    int index;
                    ds.InsertNewObjectByCriteria(ds.data, settlements.GetRandom(out index, rnd.RND), string.Format("faction\t{0},", faction), "denari");
                    settlements.RemoveAt(index);
                }
            }

            MatchCharacterCoordsToCities(factions, rnd, ds, cm);

            return "Random city allocation complete";
        }

        public static string RandCitiesVoronoi(SMF smf, RandWrap rnd, DS ds = null, CityMap cm = null)
        {
            rnd.RefreshRndSeed();

            var settlements = ds.GetItemsByIdent("settlement").DeepCopy();
            ds.DeleteValue(ds.data, "settlement");
            var factions = smf.GetFactions();
            var vp = Voronoi.GetVoronoiPoints(factions.Count, cm.width, cm.height, rnd);
            var gh = Voronoi.GetVoronoiGroups(cm.CityCoordinates, vp);

            //gh.Shuffle(TWRand.rnd);
            //factions.Shuffle(TWRand.rnd);

            for(int i =0; i < factions.Count(); i++)
            {
                foreach(var region in gh[i])
                {
                    var city = ds.GetItemByValue(settlements, region);

                    if(city != null)
                        ds.InsertNewObjectByCriteria(ds.data, city, string.Format("faction\t{0},", factions[i]), "denari");
                }
            }

            MatchCharacterCoordsToCities(factions.ToArray(), rnd, ds, cm);

            return "Rand cities voronoi complete";
        }

        private static void MatchCharacterCoordsToCities(string[] factionList, RandWrap rnd, DS ds, CityMap cm)
        {
            rnd.RefreshRndSeed();
            var factions = factionList;

            foreach(var f in factions)
            {
                var settlements = ds.GetItemsByCriteria("character", "settlement", string.Format("faction\t{0},", f));

                if (settlements.Count == 0)
                    continue;

                var regions = ds.GetItemsByCriteriaDepth(settlements, "", "region", "settlement");
                var characters = ds.GetItemsByCriteria("character_record", "character", string.Format("faction\t{0},", f));
                ChangeCharacterCoords(regions, characters, cm);
            }

        }

        private static void ChangeCharacterCoords(List<IbaseObj> regions, List<IbaseObj> characters, CityMap cm)
        {
            int ri = 0;
            foreach(BaseObj c in characters)
            {
                if (ri >= regions.Count)
                    ri = 0;

                Vector2 coord = cm.CityCoordinates[regions[ri].Value];
                c.Value = DS.ChangeCharacterCoordinates(c.Value, coord);
                ri++;
            }


        }

        


    }
}
