namespace RTWLibPlus.randomiser;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.map;
using RTWLibPlus.Modifiers;
using RTWLibPlus.parsers.objects;
using System.Collections.Generic;
using System.Numerics;

public static class RandDS
{
    public static string RandCitiesBasic(SMF smf, RandWrap rnd, DS ds = null, DR dr = null, CityMap cm = null)
    {
        rnd.RefreshRndSeed();

        List<IBaseObj> settlements = ds.GetItemsByIdent("settlement").DeepCopy();
        ds.DeleteValue(ds.Data, "settlement");
        List<string> factionList = smf.GetFactions();
        List<string> missingRegions = DRModifier.GetMissingRegionNames(settlements, dr);
        settlements.AddRange(StratModifier.CreateSettlements(settlements[1], missingRegions));
        factionList.Shuffle(rnd.RND);
        string[] factions = factionList.ToArray();
        int settlementsPerFaction = settlements.Count / factions.Length;

        while (settlements.Count > 0)
        {
            foreach (string faction in factions)
            {
                if (settlements.Count == 0)
                {
                    break;
                }

                ds.InsertNewObjectByCriteria(ds.Data, settlements.GetRandom(out int index, rnd.RND), string.Format("faction\t{0},", faction), "denari");
                settlements.RemoveAt(index);
            }
        }

        MatchCharacterCoordsToCities(factions, rnd, ds, cm);

        return "Random city allocation complete";
    }

    public static string RandCitiesVoronoi(SMF smf, RandWrap rnd, DS ds = null, DR dr = null, CityMap cm = null)
    {
        rnd.RefreshRndSeed();
        List<IBaseObj> settlements = ds.GetItemsByIdent("settlement").DeepCopy();
        List<string> missingRegions = DRModifier.GetMissingRegionNames(settlements, dr);
        settlements.AddRange(StratModifier.CreateSettlements(settlements[1], missingRegions));
        ds.DeleteValue(ds.Data, "settlement");
        List<string> factions = smf.GetFactions();
        Vector2[] vp = Voronoi.GetVoronoiPoints(factions.Count, cm.Width, cm.Height, rnd);
        List<string[]> gh = Voronoi.GetVoronoiGroups(cm.CityCoordinates, vp);

        // gh.Shuffle(TWRand.rnd);
        // factions.Shuffle(TWRand.rnd);
        // function to get missing settlements and add them to the pool
        for (int i = 0; i < factions.Count; i++)
        {
            foreach (string region in gh[i])
            {
                IBaseObj city = ds.GetItemByValue(settlements, region);

                if (city != null)
                {
                    ds.InsertNewObjectByCriteria(ds.Data, city, string.Format("faction\t{0},", factions[i]), "denari");
                }
            }
        }

        MatchCharacterCoordsToCities(factions.ToArray(), rnd, ds, cm);

        return "Rand cities voronoi complete";
    }

    private static void MatchCharacterCoordsToCities(string[] factionList, RandWrap rnd, DS ds, CityMap cm)
    {
        rnd.RefreshRndSeed();
        string[] factions = factionList;

        foreach (string f in factions)
        {
            List<IBaseObj> settlements = ds.GetItemsByCriteria("character", "settlement", string.Format("faction\t{0},", f));

            if (settlements.Count == 0)
            {
                continue;
            }

            List<IBaseObj> regions = ds.GetItemsByCriteriaDepth(settlements, "", "region", "settlement");
            List<IBaseObj> characters = ds.GetItemsByCriteria("character_record", "character", string.Format("faction\t{0},", f));
            ChangeCharacterCoords(regions, characters, cm);
        }

    }

    private static void ChangeCharacterCoords(List<IBaseObj> regions, List<IBaseObj> characters, CityMap cm)
    {
        int ri = 0;
        foreach (BaseObj c in characters)
        {
            if (ri >= regions.Count)
            {
                ri = 0;
            }

            Vector2 coord = cm.CityCoordinates[regions[ri].Value];

            c.Value = DS.ChangeCharacterCoordinates(c.Value, coord, cm.GetClosestWater(coord));
            ri++;
        }


    }
}
