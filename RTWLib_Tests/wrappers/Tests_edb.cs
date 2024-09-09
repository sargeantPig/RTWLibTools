namespace RTWLib_Tests.wrappers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.dataWrappers;
using System.Collections.Generic;
using RTWLibPlus.interfaces;

[TestClass]
public class Tests_edb
{
    [TestMethod]
    public void EdbParse()
    {
        EDB edb = Instance.InstanceEDB(TestHelper.Config, "resources", "edbExample.txt");
        string result = edb.Output();
        string expected = DepthParse.ReadFileAsString(RFH.CurrDirPath("resources", "edbExample.txt"));

        int rl = result.Length;
        int el = expected.Length;

        Assert.AreEqual(el, rl);
        Assert.AreEqual(expected, result);

    }
    [TestMethod]
    public void EdbWholeFile()
    {
        EDB edb = Instance.InstanceEDB(TestHelper.Config, TestHelper.EDB);
        string result = edb.Output();
        string expected = DepthParse.ReadFileAsString(RFH.CurrDirPath(TestHelper.EDB));

        int rl = result.Length;
        int el = expected.Length;

        Assert.AreEqual(el, rl);
        Assert.AreEqual(expected, result);

    }
    [TestMethod]
    public void EdbGetBuildingLevels()
    {
        EDB edb = Instance.InstanceEDB(TestHelper.Config, TestHelper.EDB);
        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(edb.Data, 0, "core_building", "levels");
        KeyValuePair<string, string> expected = new("levels", "governors_house governors_villa governors_palace proconsuls_palace imperial_palace");

        Assert.AreEqual(expected, result);

    }
    [TestMethod]
    public void EdbGetRequires()
    {
        EDB edb = Instance.InstanceEDB(TestHelper.Config, TestHelper.EDB);

        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(edb.Data, 0, "core_building", "levels", "governors_house");
        KeyValuePair<string, string> expected = new("governors_house", "requires factions { barbarian, carthaginian, eastern, parthia, egyptian, greek, roman, }");

        Assert.AreEqual(expected, result);

    }
    [TestMethod]
    public void EdbModifyRequires()
    {
        EDB edb = Instance.InstanceEDB(TestHelper.Config, TestHelper.EDB);

        bool change = BaseWrapper.ModifyValue(edb.Data, "requires factions { barbarian, }", 0, false, "core_building", "levels", "governors_house");
        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(edb.Data, 0, "core_building", "levels", "governors_house");
        KeyValuePair<string, string> expected = new("governors_house", "requires factions { barbarian, }");

        Assert.AreEqual(expected, result);
        Assert.AreEqual(true, change);
    }
    [TestMethod]
    public void EdbGetCapabiltyArray()
    {
        EDB edb = Instance.InstanceEDB(TestHelper.Config, TestHelper.EDB);

        List<IBaseObj> result = BaseWrapper.GetItemList(edb.Data, 0, "core_building", "levels", "governors_house", "capability");
        List<IBaseObj> expected = [

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
        ];
        Assert.AreEqual(expected.Count, result.Count);
        //TestHelper.LoopListAssert(expected, result); //broken
    }
    [TestMethod]
    public void EdbGetPopulationHealth()
    {
        EDB edb = Instance.InstanceEDB(TestHelper.Config, TestHelper.EDB);

        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(edb.Data, 0, "health", "levels", "sewers", "capability", "population_health_bonus");
        KeyValuePair<string, string> expected = new("population_health_bonus", "bonus 1");

        Assert.AreEqual(expected, result);

    }
    [TestMethod]
    public void EdbModifyPopulationHealth()
    {
        EDB edb = Instance.InstanceEDB(TestHelper.Config, TestHelper.EDB);
        bool rb = BaseWrapper.ModifyValue(edb.Data, "bonus 3", 0, false, "health", "levels", "sewers", "capability", "population_health_bonus");

        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(edb.Data, 0, "health", "levels", "sewers", "capability", "population_health_bonus");
        KeyValuePair<string, string> expected = new("population_health_bonus", "bonus 3");

        Assert.AreEqual(expected, result);
        Assert.AreEqual(true, rb);
    }

}
