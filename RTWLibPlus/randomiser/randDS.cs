﻿namespace RTWLibPlus.randomiser;

using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.map;
using RTWLibPlus.Modifiers;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public static class RandDS
{
    public static string RandCitiesBasic(SMF smf, RandWrap rnd, DS ds = null, DR dr = null, CityMap cm = null)
    {
        rnd.RefreshRndSeed();

        List<IBaseObj> settlements = ds.GetItemsByIdent("settlement").DeepCopy();
        BaseWrapper.DeleteValue(ds.Data, "settlement");
        List<string> factionList = smf.GetFactions();
        List<string> missingRegions = DRModifier.GetMissingRegionNames(settlements, dr);
        settlements.AddRange(StratModifier.CreateSettlements(settlements[1], missingRegions));
        factionList.Shuffle(RandWrap.RND);
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

                BaseWrapper.InsertNewObjectByCriteria(ds.Data, settlements.GetRandom(out int index, RandWrap.RND), string.Format("faction\t{0},", faction), "denari");
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
        BaseWrapper.DeleteValue(ds.Data, "settlement");
        List<string> factions = smf.GetFactions();
        Vector2[] vp = Voronoi.GetVoronoiPoints(factions.Count, cm.Width, cm.Height, rnd);
        List<string[]> gh = Voronoi.GetVoronoiGroups(cm.CityCoordinates, vp);

        for (int i = 0; i < factions.Count; i++)
        {
            if (gh[i].Length == 0)
            {
                Console.WriteLine("no settlements in group");
            }

            foreach (string region in gh[i])
            {
                IBaseObj city = ds.GetItemByValue(settlements, region);

                if (city != null)
                {
                    BaseWrapper.InsertNewObjectByCriteria(ds.Data, city, string.Format("faction\t{0},", factions[i]), "denari");
                }
            }
        }

        MatchCharacterCoordsToCities(factions.ToArray(), rnd, ds, cm);

        return "Rand cities voronoi complete";
    }

    public static string SwitchUnitsToRecruitable(EDU edu, DS ds, RandWrap rnd)
    {
        rnd.RefreshRndSeed();
        List<IBaseObj> factions = ds.GetItemsByIdent("faction");
        foreach (IBaseObj faction in factions)
        {
            string name = faction.Tag.RemoveFirstWord('\t').Trim(',');
            List<string> units = edu.GetUnitsFromFaction(name, ["civ", "female", "naval"]);
            List<IBaseObj> dsunits = ds.GetItemsByCriteria("character_record", "unit", faction.Tag, "character", "army");
            string[] generalFilter = ["general", "generals", "general's", "chieftain", "bodyguard"];
            for (int i = 0; i < dsunits.Count; i++)
            {
                bool skip = generalFilter.Any(sub => dsunits[i].Tag.Contains(sub) || dsunits[i].Value.Contains(sub));

                if (i == 0 || dsunits[i].Tag.Contains("naval") || skip)
                {
                    continue;
                }
                IBaseObj dsunit = dsunits[i];

                string randUnit = units.GetRandom(out int index, RandWrap.RND);
                IBaseObj newunit = StratModifier.CreateUnit(dsunit, randUnit);
                dsunit.Value = newunit.Value;
                dsunit.Tag = newunit.Tag;
                dsunits[i] = dsunit;
            }
        }

        return "Units switched to units in a given factions recruitment pool";
    }

    public static string RandRelations(DS ds, SMF smf, CityMap cm, int range)
    {
        ds.DeleteChunks("script", "core_attitudes");

        List<string> factions = smf.GetFactions();
        List<string> factionsCopy = smf.GetFactions();
        Dictionary<string, string[]> factionRegions = ds.GetFactionRegionDict();
        Dictionary<string, string[]> closeFactions = cm.GetClosestRegions(factionRegions, range);
        Dictionary<string, string[]> closeFactionsCopy = closeFactions;
        RandRelationshipHandle(ds, factions, closeFactions, true);
        RandRelationshipHandle(ds, factionsCopy, closeFactionsCopy, false);
        return "Relationships randomise";
    }

    private static void RandRelationshipHandle(DS ds, List<string> factions, Dictionary<string, string[]> closeFactions, bool core)
    {
        int[] relation_values = [0, 100, 200, 400, 600];
        for (int i = 0; i < factions.Count; i++)
        {
            List<IBaseObj> faction_cities = ds.GetItemsByCriteriaSimpleDepth(ds.Data, "character", "region", [], string.Format("faction\t{0},", factions[i]));
            foreach (string closeFaction in closeFactions[factions[i]])
            {
                DSObj newRelation;
                if (core)
                {
                    newRelation = new(
                        StratModifier.CreateFactionCoreAttitude(factions[i], closeFaction, relation_values.GetRandom(out int tag, RandWrap.RND)),
                        StratModifier.CreateFactionCoreAttitude(factions[i], closeFaction, relation_values[tag]),
                        0);
                }
                else
                {
                    newRelation = new(
                        StratModifier.CreateFactionRelation(factions[i], closeFaction, relation_values.GetRandom(out int tag, RandWrap.RND)),
                        StratModifier.CreateFactionRelation(factions[i], closeFaction, relation_values[tag]),
                        0);
                }

                ds.InsertAt(ds.Data.Count - 2, newRelation);
                closeFactions[closeFaction] = closeFactions[closeFaction].FindAndRemove(factions[i]);
            }

            factions.RemoveAt(0);
            i = 0;
        }
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
            Vector2 waterCoord = coord;
            if (c.Value.Contains("admiral"))
            {
                waterCoord = cm.GetClosestWater(coord);
                coord = waterCoord;
            }

            c.Value = StratModifier.ChangeCharacterCoordinates(c.Value, coord);
            ri++;
        }
    }
}
