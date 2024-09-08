namespace RTWLibPlus.Modifiers;

using System.Collections.Generic;
using System.Numerics;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;

public class StratModifier
{
    public StratModifier()
    { }

    /// <summary>
    /// Create a settlement given the region name, also requires a dummy settlement to copy the structure
    /// </summary>
    /// <returns>IBaseObj containing settlement structure</returns>
    public static IBaseObj CreateSettlement(IBaseObj dummySettlement, string regionName)
    {
        DSObj dummy = (DSObj)dummySettlement.Copy();
        dummy.FindAndModify("region", regionName);

        return dummy;
    }

    public static List<IBaseObj> CreateSettlements(IBaseObj dummySettlement, List<string> regionNames)
    {
        List<IBaseObj> settlements = [];
        foreach (string region in regionNames)
        {
            settlements.Add(CreateSettlement(dummySettlement, region));
        }
        return settlements;
    }

    public static void AddSettlementToFaction(IBaseObj faction, IBaseObj settlement)
    {
        int index = faction.FirstOfIndex("settlement");
        faction.InsertToItems(settlement, index);
    }

    public static IBaseObj CreateBuilding(IBaseObj dummyBuilding, string building)
    {
        DSObj dummy = (DSObj)dummyBuilding.Copy();
        dummy.FindAndModify("type", building);
        return dummy;
    }

    public static void AddBuildingToSettlement(IBaseObj settlement, IBaseObj building) => settlement.AddToItems(building);

    public static IBaseObj CreateUnit(IBaseObj dummyUnit, string unit)
    {
        DSObj dummy = (DSObj)dummyUnit.Copy();
        string firstWord = unit.GetFirstWord(' ');
        string tag = string.Format("unit\t\t\t{0}", firstWord);
        string value = string.Format("{0}\t\t\t\texp 1 armour 0 weapon_lvl 0", unit.RemoveFirstWord(' '));
        dummy.Tag = tag;
        dummy.Value = value;
        return dummy;
    }


    public static string ChangeCharacterCoordinates(string character, Vector2 coords)
    {
        string[] split = character.Split(',').TrimAll();
        string x = string.Format("x {0}", (int)coords.X);
        string y = string.Format("y {0}", (int)coords.Y);
        split[^1] = y;
        split[^2] = x;
        return split.ToString(',', ' ');
    }

    public static string CreateFactionCoreAttitude(string factionA, string factionB, int relation) => string.Format("core_attitudes\t{0},\t{1}\t\t{2}", factionA, relation, factionB);

    public static string CreateFactionRelation(string factionA, string factionB, int relation) => string.Format("faction_relationships\t{0},\t{1}\t\t{2}", factionA, relation, factionB);
}

