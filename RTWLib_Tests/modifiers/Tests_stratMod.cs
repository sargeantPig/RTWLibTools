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
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> settlements = descr_strat.GetItemsByIdent("settlement").DeepCopy();
        IBaseObj modifiedSettlement = StratModifier.CreateSettlement(settlements[0], "test_name");
        string result = modifiedSettlement.Find("region");
        Assert.AreEqual("test_name", result);
    }

    [TestMethod]
    public void CreateBuilding()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> settlements = descr_strat.GetItemsByIdent("settlement").DeepCopy();
        IBaseObj modifiedBuilding = StratModifier.CreateBuilding(settlements[0].GetObject("building"), "test_name");
        string result = modifiedBuilding.Find("type");
        Assert.AreEqual("test_name", result);
    }

    [TestMethod]
    public void AddSettlementToFaction()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> settlements = descr_strat.GetItemsByCriteria("character", "settlement", "faction\tromans_julii,");
        IBaseObj modifiedSettlement = StratModifier.CreateSettlement(settlements[0], "test_name");
        int placeAt = BaseWrapper.GetIndexByCriteria(descr_strat.Data, "faction\tromans_julii,", "settlement");
        descr_strat.InsertAt(placeAt + 1, modifiedSettlement);
        List<IBaseObj> result = descr_strat.GetItemsByCriteria("character", "settlement", "faction\tromans_julii,");
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("test_name", result[1].Find("region"));
    }

    [TestMethod]
    public void AddSettlementToFaction2()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> settlements = descr_strat.GetItemsByCriteria("character", "settlement", "faction\tmacedon,");
        IBaseObj modifiedSettlement = StratModifier.CreateSettlement(settlements[0], "test_name");
        int placeAt = BaseWrapper.GetIndexByCriteria(descr_strat.Data, "faction\tmacedon,", "settlement");
        descr_strat.InsertAt(placeAt + 1, modifiedSettlement);
        List<IBaseObj> result = descr_strat.GetItemsByCriteria("character", "settlement", "faction\tmacedon,");
        Assert.AreEqual(5, result.Count);
        Assert.AreEqual("test_name", result[1].Find("region"));
    }

    [TestMethod]
    public void CreateUnit()
    {
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> units = descr_strat.GetItemsByIdent("unit").DeepCopy();
        IBaseObj result = StratModifier.CreateUnit(units[0], "town watch");
        Assert.AreEqual("unit", result.Ident);
        Assert.AreEqual("unit\t\t\ttown", result.Tag);
        Assert.AreEqual("watch\t\t\t\texp 1 armour 0 weapon_lvl 0", result.Value);
    }
}
