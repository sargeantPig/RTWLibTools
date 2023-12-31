namespace RTWLib_Tests.Modifiers;

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.Modifiers;

[TestClass]
public class TestsStrat
{
    private readonly DepthParse dp = new();
    private readonly TWConfig config = TWConfig.LoadConfig(@"resources/remaster.json");

    [TestMethod]
    public void CreateSettlement()
    {
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        List<IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);
        List<IBaseObj> settlements = parsedds.GetItemsByIdent("settlement").DeepCopy();
        IBaseObj modifiedSettlement = StratModifier.CreateSettlement(settlements[0], "test_name");
        string result = modifiedSettlement.Find("region");
        Assert.AreEqual("test_name", result);
    }

    [TestMethod]
    public void CreateBuilding()
    {
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        List<IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);
        List<IBaseObj> settlements = parsedds.GetItemsByIdent("settlement").DeepCopy();
        IBaseObj modifiedBuilding = StratModifier.CreateBuilding(settlements[0].GetObject("building"), "test_name");
        string result = modifiedBuilding.Find("type");
        Assert.AreEqual("test_name", result);
    }

    [TestMethod]
    public void AddSettlementToFaction()
    {
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        List<IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);
        List<IBaseObj> settlements = parsedds.GetItemsByCriteria("character", "settlement", "romans_julii");
        IBaseObj modifiedSettlement = StratModifier.CreateSettlement(settlements[0], "test_name");
        int placeAt = parsedds.GetIndexByCriteria(parsedds.Data, "romans_julii", "settlement");
        parsedds.InsertAt(placeAt + 1, modifiedSettlement);
        List<IBaseObj> result = parsedds.GetItemsByCriteria("character", "settlement", "romans_julii");
        Assert.AreEqual(3, result.Count);
    }
}
