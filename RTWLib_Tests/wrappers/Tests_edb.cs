using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers;
using RTWLibPlus.parsers.objects;
using System.IO;
using RTWLibPlus.dataWrappers;
using System.Collections.Generic;
using RTWLibPlus.interfaces;

namespace RTWLib_Tests.wrappers
{
    [TestClass]
    public class Tests_edb
    {
        DepthParse dp = new DepthParse();

        [TestMethod]
        public void edbParse()
        {
            var edb = dp.ReadFile(Path.Combine("resources", "edbExample.txt"), false);
            var edbParse = dp.Parse(edb, Creator.EDBcreator);
            var parsedEdb = new EDB(edbParse);

            string result = parsedEdb.Output();
            var expected = dp.ReadFileAsString(RFH.CurrDirPath("resources", "edbExample.txt"));

            int rl = result.Length;
            int el = expected.Length;

            Assert.AreEqual(el, rl);
            Assert.AreEqual(expected, result);

        }
        [TestMethod]
        public void edbWholeFile()
        {
            var edb = dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_buildings.txt"));
            var edbParse = dp.Parse(edb, Creator.EDBcreator);
            var parsedEdb = new EDB(edbParse);

            string result = parsedEdb.Output();
            var expected = dp.ReadFileAsString(RFH.CurrDirPath("resources", "export_descr_buildings.txt"));

            int rl = result.Length;
            int el = expected.Length;

            Assert.AreEqual(el, rl);
            Assert.AreEqual(expected, result);

        }
        [TestMethod]
        public void edbGetBuildingLevels()
        {
            var smf = dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_buildings.txt"), false);
            var smfParse = dp.Parse(smf, Creator.EDBcreator);
            var parsedsmf = new EDB(smfParse);

            var result = parsedsmf.GetKeyValueAtLocation(parsedsmf.data, 0, "core_building", "levels");
            var expected = new KeyValuePair<string, string>("levels", "governors_house governors_villa governors_palace proconsuls_palace imperial_palace");

            Assert.AreEqual(expected, result);

        }
        [TestMethod]
        public void edbGetRequires()
        {
            var smf = dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_buildings.txt"), false);
            var smfParse = dp.Parse(smf, Creator.EDBcreator);
            var parsedsmf = new EDB(smfParse);

            var result = parsedsmf.GetKeyValueAtLocation(parsedsmf.data, 0, "core_building", "levels", "governors_house");
            var expected = new KeyValuePair<string, string>("governors_house", "requires factions { barbarian, carthaginian, eastern, parthia, egyptian, greek, roman, }");

            Assert.AreEqual(expected, result);

        }
        [TestMethod]
        public void edbModifyRequires()
        {
            var smf = dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_buildings.txt"), false);
            var smfParse = dp.Parse(smf, Creator.EDBcreator);
            var parsedsmf = new EDB(smfParse);

            var change = parsedsmf.ModifyValue(parsedsmf.data, "requires factions { barbarian, }", 0, false, "core_building", "levels", "governors_house");
            var result = parsedsmf.GetKeyValueAtLocation(parsedsmf.data, 0, "core_building", "levels", "governors_house");
            var expected = new KeyValuePair<string, string>("governors_house", "requires factions { barbarian, }");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(true, change);
        }
        [TestMethod]
        public void edbGetCapabiltyArray()
        {
            var smf = dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_buildings.txt"), false);
            var smfParse = dp.Parse(smf, Creator.EDBcreator);
            var parsedsmf = new EDB(smfParse);

            var result = parsedsmf.GetItemList(parsedsmf.data, 0, "core_building", "levels", "governors_house", "capability");
            var expected = new List<IbaseObj>() {

                 new SMFObj("recruit", "\"carthaginian peasant\"  0", 4),
                 new SMFObj("recruit", "\"barb peasant briton\"  0", 4),
                 new SMFObj("recruit", "\"barb peasant dacian\"  0", 4),
                 new SMFObj("recruit", "\"barb peasant gaul\"  0", 4),
                 new SMFObj("recruit", "\"barb peasant german\"  0", 4),
                 new SMFObj("recruit", "\"barb peasant scythian\"  0", 4),
                 new SMFObj("recruit", "\"east peasant\"  0", 4),
                 new SMFObj("recruit", "\"egyptian peasant\"  0", 4),
                 new SMFObj("recruit", "\"greek peasant\"  0", 4),
                 new SMFObj("recruit", "\"roman peasant\"  0", 4),
            };

            TestHelper.LoopListAssert(expected, result);
        }
        [TestMethod]
        public void edbGetPopulationHealth()
        {
            var smf = dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_buildings.txt"), false);
            var smfParse = dp.Parse(smf, Creator.EDBcreator);
            var parsedsmf = new EDB(smfParse);

            var result = parsedsmf.GetKeyValueAtLocation(parsedsmf.data, 0, "health", "levels", "sewers", "capability", "population_health_bonus");
            var expected = new KeyValuePair<string, string>("population_health_bonus", "bonus 1");

            Assert.AreEqual(expected, result);

        }
        [TestMethod]
        public void edbModifyPopulationHealth()
        {
            var smf = dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_buildings.txt"), false);
            var smfParse = dp.Parse(smf, Creator.EDBcreator);
            var parsedsmf = new EDB(smfParse);
            bool rb = parsedsmf.ModifyValue(parsedsmf.data, "bonus 3",  0, false, "health", "levels", "sewers", "capability", "population_health_bonus");

            var result = parsedsmf.GetKeyValueAtLocation(parsedsmf.data, 0, "health", "levels", "sewers", "capability", "population_health_bonus");
            var expected = new KeyValuePair<string, string>("population_health_bonus", "bonus 3");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(true, rb);
        }

    }
}
