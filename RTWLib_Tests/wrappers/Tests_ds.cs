﻿namespace RTWLib_Tests.wrappers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers;
using RTWLibPlus.parsers.objects;
using System.Numerics;

[TestClass]
public class Tests_ds
{
    private readonly DepthParse dp = new();
    private readonly TWConfig config = TWConfig.LoadConfig(@"resources/remaster.json");
    [TestMethod]
    public void DsWholeFile()
    {

        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);

        string result = parsedds.Output();

        string expected = this.dp.ReadFileAsString(RFH.CurrDirPath("resources", "descr_strat.txt"));

        int rl = result.Length;  //123502
        int el = expected.Length; //127957

        Assert.AreEqual(el, rl);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void DsGetItemsByIdentSettlements()
    {
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);

        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> result = parsedds.GetItemsByIdent("settlement");
        int expected = 96; //number of settlements

        Assert.AreEqual(expected, result.Count); //check number of returned settlements
    }

    [TestMethod]
    public void DsGetCharacterChangeCoords()
    {
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> characters = parsedds.GetItemsByIdent("character");
        string result = DS.ChangeCharacterCoordinates(((BaseObj)characters[0]).Value, new Vector2(1, 1));
        string expected = "Julius, named character, leader, age 47, , x 1, y 1"; //number of settlements

        Assert.AreEqual(expected, result); //check number of returned settlements
    }

    [TestMethod]
    public void DsGetItemsByIdentResource()
    {
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);

        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> result = parsedds.GetItemsByIdent("resource");
        int expected = 300; //number of resources

        Assert.AreEqual(expected, result.Count); //check number of returned resources
    }
    [TestMethod]
    public void DsGetItemsByIdentFaction()
    {
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);

        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> result = parsedds.GetItemsByIdent("faction");
        int expected = 21; //number of factions

        Assert.AreEqual(expected, result.Count); //check number of returned factions
    }
    [TestMethod]
    public void DsGetItemsByIdentCoreAttitudes()
    {
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);

        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> result = parsedds.GetItemsByIdent("core_attitudes");
        int expected = 47; //number of ca

        Assert.AreEqual(expected, result.Count); //check number of returned ca
    }
    [TestMethod]
    public void DsDeleteByIdent()
    {
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);

        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> settlements = parsedds.GetItemsByIdent("settlement");
        parsedds.DeleteValue(parsedds.Data, "settlement");
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> result = parsedds.GetItemsByIdent("settlement");
        int expected = 0; //number of ca

        Assert.AreEqual(expected, result.Count); //check number of returned ca
    }
    [TestMethod]
    public void DsAddSettlementToRomansBrutii()
    {
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);

        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> settlements = parsedds.GetItemsByIdent("settlement");
        bool add = parsedds.InsertNewObjectByCriteria(parsedds.Data, settlements[30], "faction\tromans_brutii,", "denari");
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> result = parsedds.GetItemsByCriteria("character", "settlement", "faction\tromans_brutii,");
        int expected = 3; //number of ca

        Assert.AreEqual(expected, result.Count); //check number of returned ca
    }
    [TestMethod]
    public void DsAddSettlementToScythia()
    {
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);

        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> settlements = parsedds.GetItemsByIdent("settlement");
        bool add = parsedds.InsertNewObjectByCriteria(parsedds.Data, settlements[30], "faction\tscythia,", "denari");
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> result = parsedds.GetItemsByCriteria("character", "settlement", "faction\tscythia,");
        int expected = 5; //number of ca
        Assert.AreEqual(expected, result.Count); //check number of returned ca
    }
    [TestMethod]
    public void DsAddUnitToFlavius()
    {
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> units = parsedds.GetItemsByCriteria("character", "unit", "faction\tromans_julii,", "character", "army");
        bool add = parsedds.InsertNewObjectByCriteria(parsedds.Data, units[1], "faction\tromans_julii,", "character\tFlavius", "unit");
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> result = parsedds.GetItemsByCriteria("character", "unit", "character\tFlavius", "army");
        int expected = 6; //number of ca
        Assert.AreEqual(expected, result.Count); //check number of returned ca
    }

    [TestMethod]
    public void GetRegions()
    {
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> regions = parsedds.GetItemsByCriteriaDepth(parsedds.Data, "core_attitudes", "region", "settlement");

        int result = regions.Count;
        int expected = 96; //number of ca

        Assert.AreEqual(expected, result); //check number of returned ca
    }
    [TestMethod]
    public void GetFactionByRegion()
    {
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);
        string region = "Paionia";
        string faction = parsedds.GetFactionByRegion(region);

        string result = faction;
        string expected = "macedon"; //number of ca

        Assert.AreEqual(expected, result); //check number of returned ca
    }
}
