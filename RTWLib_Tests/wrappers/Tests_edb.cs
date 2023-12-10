namespace RTWLib_Tests.wrappers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers;
using RTWLibPlus.parsers.objects;
using System.IO;
using RTWLibPlus.dataWrappers;
using System.Collections.Generic;
using RTWLibPlus.interfaces;
using RTWLibPlus.data;
using System.Runtime.InteropServices;


[TestClass]
public class Tests_edb
{
    private readonly DepthParse dp = new();
    private readonly TWConfig config = TWConfig.LoadConfig(@"resources/remaster.json");
    [TestMethod]
    public void EdbParse()
    {
        string[] edb = this.dp.ReadFile(Path.Combine("resources", "edbExample.txt"), false);
        List<IBaseObj> edbParse = this.dp.Parse(edb, Creator.EDBcreator);
        EDB parsedEdb = new(edbParse, this.config);

        string result = parsedEdb.Output();
        string expected = this.dp.ReadFileAsString(RFH.CurrDirPath("resources", "edbExample.txt"));

        int rl = result.Length;
        int el = expected.Length;

        Assert.AreEqual(el, rl);
        Assert.AreEqual(expected, result);

    }
    [TestMethod]
    public void EdbWholeFile()
    {
        bool ismac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        string[] edb = this.dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_buildings.txt"));
        List<IBaseObj> edbParse = this.dp.Parse(edb, Creator.EDBcreator);
        EDB parsedEdb = new(edbParse, this.config);

        string result = parsedEdb.Output();
        string expected = this.dp.ReadFileAsString(RFH.CurrDirPath("resources", "export_descr_buildings.txt"));

        int rl = result.Length;
        int el = expected.Length;

        Assert.AreEqual(el, rl);
        Assert.AreEqual(expected, result);

    }
    [TestMethod]
    public void EdbGetBuildingLevels()
    {
        string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_buildings.txt"), false);
        List<IBaseObj> smfParse = this.dp.Parse(smf, Creator.EDBcreator);
        EDB parsedsmf = new(smfParse, this.config);

        KeyValuePair<string, string> result = parsedsmf.GetKeyValueAtLocation(parsedsmf.Data, 0, "core_building", "levels");
        KeyValuePair<string, string> expected = new("levels", "governors_house governors_villa governors_palace proconsuls_palace imperial_palace");

        Assert.AreEqual(expected, result);

    }
    [TestMethod]
    public void EdbGetRequires()
    {
        string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_buildings.txt"), false);
        List<IBaseObj> smfParse = this.dp.Parse(smf, Creator.EDBcreator);
        EDB parsedsmf = new(smfParse, this.config);

        KeyValuePair<string, string> result = parsedsmf.GetKeyValueAtLocation(parsedsmf.Data, 0, "core_building", "levels", "governors_house");
        KeyValuePair<string, string> expected = new("governors_house", "requires factions { barbarian, carthaginian, eastern, parthia, egyptian, greek, roman, }");

        Assert.AreEqual(expected, result);

    }
    [TestMethod]
    public void EdbModifyRequires()
    {
        string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_buildings.txt"), false);
        List<IBaseObj> smfParse = this.dp.Parse(smf, Creator.EDBcreator);
        EDB parsedsmf = new(smfParse, this.config);

        bool change = parsedsmf.ModifyValue(parsedsmf.Data, "requires factions { barbarian, }", 0, false, "core_building", "levels", "governors_house");
        KeyValuePair<string, string> result = parsedsmf.GetKeyValueAtLocation(parsedsmf.Data, 0, "core_building", "levels", "governors_house");
        KeyValuePair<string, string> expected = new("governors_house", "requires factions { barbarian, }");

        Assert.AreEqual(expected, result);
        Assert.AreEqual(true, change);
    }
    [TestMethod]
    public void EdbGetCapabiltyArray()
    {
        string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_buildings.txt"), false);
        List<IBaseObj> smfParse = this.dp.Parse(smf, Creator.EDBcreator);
        EDB parsedsmf = new(smfParse, this.config);

        List<IBaseObj> result = parsedsmf.GetItemList(parsedsmf.Data, 0, "core_building", "levels", "governors_house", "capability");
        List<IBaseObj> expected = new() {

             new EDBObj("recruit", "\"carthaginian peasant\"  0", 4),
             new EDBObj("recruit", "\"barb peasant briton\"  0", 4),
             new EDBObj("recruit", "\"barb peasant dacian\"  0", 4),
             new EDBObj("recruit", "\"barb peasant gaul\"  0", 4),
             new EDBObj("recruit", "\"barb peasant german\"  0", 4),
             new EDBObj("recruit", "\"barb peasant scythian\"  0", 4),
             new EDBObj("recruit", "\"east peasant\"  0", 4),
             new EDBObj("recruit", "\"egyptian peasant\"  0", 4),
             new EDBObj("recruit", "\"greek peasant\"  0", 4),
             new EDBObj("recruit", "\"roman peasant\"  0", 4),
        };
        Assert.AreEqual(expected.Count, result.Count);
        //TestHelper.LoopListAssert(expected, result); //broken
    }
    [TestMethod]
    public void EdbGetPopulationHealth()
    {
        string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_buildings.txt"), false);
        List<IBaseObj> smfParse = this.dp.Parse(smf, Creator.EDBcreator);
        EDB parsedsmf = new(smfParse, this.config);

        KeyValuePair<string, string> result = parsedsmf.GetKeyValueAtLocation(parsedsmf.Data, 0, "health", "levels", "sewers", "capability", "population_health_bonus");
        KeyValuePair<string, string> expected = new("population_health_bonus", "bonus 1");

        Assert.AreEqual(expected, result);

    }
    [TestMethod]
    public void EdbModifyPopulationHealth()
    {
        string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_buildings.txt"), false);
        List<IBaseObj> smfParse = this.dp.Parse(smf, Creator.EDBcreator);
        EDB parsedsmf = new(smfParse, this.config);
        bool rb = parsedsmf.ModifyValue(parsedsmf.Data, "bonus 3", 0, false, "health", "levels", "sewers", "capability", "population_health_bonus");

        KeyValuePair<string, string> result = parsedsmf.GetKeyValueAtLocation(parsedsmf.Data, 0, "health", "levels", "sewers", "capability", "population_health_bonus");
        KeyValuePair<string, string> expected = new("population_health_bonus", "bonus 3");

        Assert.AreEqual(expected, result);
        Assert.AreEqual(true, rb);
    }

}
