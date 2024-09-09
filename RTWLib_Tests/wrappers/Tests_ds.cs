namespace RTWLib_Tests.wrappers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.dataWrappers.TGA;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.map;
using RTWLibPlus.Modifiers;
using RTWLibPlus.parsers;
using RTWLibPlus.parsers.objects;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;


[TestClass]
public class Tests_ds
{
    [TestMethod]
    public void DsWholeFile()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        string result = descr_strat.Output();
        string expected = DepthParse.ReadFileAsString(RFH.CurrDirPath(TestHelper.DS));
        int rl = result.Length;  //123502
        int el = expected.Length; //127957

        Assert.AreEqual(el, rl);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void DsGetItemsByIdentSettlements()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> result = descr_strat.GetItemsByIdent("settlement");
        int expected = 96; //number of settlements
        Assert.AreEqual(expected, result.Count); //check number of returned settlements
    }

    [TestMethod]
    public void DsGetCharacterChangeCoords()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> characters = descr_strat.GetItemsByIdent("character");
        string result = StratModifier.ChangeCharacterCoordinates(((BaseObj)characters[0]).Value, new Vector2(1, 1));
        string expected = "Julius, named character, leader, age 47, , x 1, y 1"; //number of settlements

        Assert.AreEqual(expected, result); //check number of returned settlements
    }

    [TestMethod]
    public void DsGetItemsByIdentResource()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> result = descr_strat.GetItemsByIdent("resource");
        int expected = 300; //number of resources

        Assert.AreEqual(expected, result.Count); //check number of returned resources
    }
    [TestMethod]
    public void DsGetItemsByIdentFaction()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> result = descr_strat.GetItemsByIdent("faction");
        int expected = 21; //number of factions

        Assert.AreEqual(expected, result.Count); //check number of returned factions
    }
    [TestMethod]
    public void DsGetItemsByIdentCoreAttitudes()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> result = descr_strat.GetItemsByIdent("core_attitudes");
        int expected = 47; //number of ca

        Assert.AreEqual(expected, result.Count); //check number of returned ca
    }
    [TestMethod]
    public void DsDeleteByIdent()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> settlements = descr_strat.GetItemsByIdent("settlement");
        BaseWrapper.DeleteValue(descr_strat.Data, "settlement");
        List<IBaseObj> result = descr_strat.GetItemsByIdent("settlement");
        int expected = 0; //number of ca

        Assert.AreEqual(expected, result.Count); //check number of returned ca
    }
    [TestMethod]
    public void DsAddSettlementToRomansBrutii()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> settlements = descr_strat.GetItemsByIdent("settlement");
        bool add = BaseWrapper.InsertNewObjectByCriteria(descr_strat.Data, settlements[30], "faction\tromans_brutii,", "denari");
        List<IBaseObj> result = descr_strat.GetItemsByCriteria("character", "settlement", "faction\tromans_brutii,");
        int expected = 3; //number of ca

        Assert.AreEqual(expected, result.Count); //check number of returned ca
    }
    [TestMethod]
    public void DsAddSettlementToScythia()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);

        List<IBaseObj> settlements = descr_strat.GetItemsByIdent("settlement");
        bool add = BaseWrapper.InsertNewObjectByCriteria(descr_strat.Data, settlements[30], "faction\tscythia,", "denari");
        List<IBaseObj> result = descr_strat.GetItemsByCriteria("character", "settlement", "faction\tscythia,");
        int expected = 5; //number of ca
        Assert.AreEqual(expected, result.Count); //check number of returned ca
    }
    [TestMethod]
    public void DsAddUnitToFlaviusAbstracted()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> units = descr_strat.GetItemsByCriteria("character", "unit", "faction\tromans_julii,", "character", "army");
        List<IBaseObj> character = descr_strat.GetItemsByCriteria("army", "character", "faction\tromans_julii,");
        List<IBaseObj> faction = descr_strat.GetItemsByIdent("faction");
        descr_strat.AddUnitToArmy(faction[0], character[0], units[0]);
        List<IBaseObj> result = descr_strat.GetItemsByCriteria("character", "unit", "character\tFlavius", "army");
        int expected = 6; //number of ca
        Assert.AreEqual(expected, result.Count); //check number of returned ca
    }

    [TestMethod]
    public void GetCharactersOfFaction()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> characters = descr_strat.GetItemsByCriteria("character_record", "character", "faction\tromans_julii,");
        int expected = 7; //number of ca
        Assert.AreEqual(expected, characters.Count); //check number of returned ca
    }

    [TestMethod]
    public void DsAddUnitToFlavius()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> units = descr_strat.GetItemsByCriteria("character", "unit", "faction\tromans_julii,", "character", "army");
        bool add = BaseWrapper.InsertNewObjectByCriteria(descr_strat.Data, units[1], "faction\tromans_julii,", "character\tFlavius", "unit");
        List<IBaseObj> result = descr_strat.GetItemsByCriteria("character", "unit", "character\tFlavius", "army");
        int expected = 6; //number of ca
        Assert.AreEqual(expected, result.Count); //check number of returned ca
    }

    [TestMethod]
    public void GetRegions()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> regions = descr_strat.GetItemsByCriteriaDepth(descr_strat.Data, "core_attitudes", "region", "settlement");

        int result = regions.Count;
        int expected = 96; //number of ca

        Assert.AreEqual(expected, result); //check number of returned ca
    }
    [TestMethod]
    public void GetFactionByRegion()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        string region = "Paionia";
        string faction = descr_strat.GetFactionByRegion(region);

        string result = faction;
        string expected = "macedon"; //number of ca

        Assert.AreEqual(expected, result); //check number of returned ca
    }
    [TestMethod]
    public void GetSettlementsForFaction()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> faction_cities = descr_strat.GetItemsByCriteriaSimpleDepth(descr_strat.Data, "character", "region", [], "faction\tromans_julii,");
        int expected = 2;
        int result = faction_cities.Count;
        Assert.AreEqual(expected, result); //check number of returned ca
    }
    [TestMethod]
    public void GetFactionRegionsDict()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        TGA image = new("tgafile", RFH.CurrDirPath("resources", "map_regions.tga"), "");
        DR dr = (DR)RFH.CreateWrapper(Creator.DRcreator, Creator.DRWrapper, TestHelper.Config, '\t', false, "resources", "descr_regions.txt");

        CityMap cm = new(image, dr);
        Dictionary<string, string[]> fr = descr_strat.GetFactionRegionDict();

        string[] expectedFirst = ["Etruria", "Umbria"];
        string[] expectedSecond = ["Gallaecia", "Lusitania", "Hispania", "Taraconenis"];
        Assert.IsTrue(expectedFirst.SequenceEqual(fr["romans_julii"]));
        Assert.IsTrue(expectedSecond.SequenceEqual(fr["spain"]));

        Dictionary<string, string[]> closeFactions = cm.GetClosestRegions(fr, 30);

        string[] closestMockOne = ["romans_julii", "romans_brutii", "romans_scipii", "carthage", "gauls", "greek_cities", "slave"];
        string[] closestMockTwo = ["gauls", "germans", "slave"];

        Assert.IsTrue(closestMockOne.SequenceEqual(closeFactions["romans_senate"]));
        Assert.IsTrue(closestMockTwo.SequenceEqual(closeFactions["britons"]));
    }
    [TestMethod]
    public void RemoveSuperFaction()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        descr_strat.RemoveSuperFaction();
        List<IBaseObj> results = descr_strat.GetItemsByIdent("superfaction");
        Assert.AreEqual(0, results.Count);
    }
}
