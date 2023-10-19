using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers;
using RTWLibPlus.parsers.objects;
using System.Numerics;

namespace RTWLib_Tests.wrappers
{
    [TestClass]
    public class Tests_ds
    {
        DepthParse dp = new DepthParse();
        TWConfig config = TWConfig.LoadConfig(@"resources/remaster.json");
        [TestMethod]
        public void dsWholeFile()
        {
            
            var ds = dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = dp.Parse(ds, Creator.DScreator);
            var parsedds = new DS(dsParse, config);

            string result = parsedds.Output();
            
            var expected = dp.ReadFileAsString(RFH.CurrDirPath("resources", "descr_strat.txt"));

            int rl = result.Length;  //123502
            int el = expected.Length; //127957

            Assert.AreEqual(el, rl);
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void dsGetItemsByIdentSettlements()
        {
            var ds = dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = dp.Parse(ds, Creator.DScreator);
            var parsedds = new DS(dsParse, config);

            var result = parsedds.GetItemsByIdent("settlement");
            var expected = 96; //number of settlements

            Assert.AreEqual(expected, result.Count); //check number of returned settlements
        }

        [TestMethod]
        public void dsGetCharacterChangeCoords()
        {
            var ds = dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = dp.Parse(ds, Creator.DScreator);
            var parsedds = new DS(dsParse, config);
            var characters = parsedds.GetItemsByIdent("character");
            string result = DS.ChangeCharacterCoordinates(((BaseObj)characters[0]).Value, new Vector2(1, 1 ));
            var expected = "Julius, named character, leader, age 47, , x 1, y 1"; //number of settlements

            Assert.AreEqual(expected, result); //check number of returned settlements
        }

        [TestMethod]
        public void dsGetItemsByIdentResource()
        {
            var ds = dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = dp.Parse(ds, Creator.DScreator);
            var parsedds = new DS(dsParse, config);

            var result = parsedds.GetItemsByIdent("resource");
            var expected = 300; //number of resources

            Assert.AreEqual(expected, result.Count); //check number of returned resources
        }
        [TestMethod]
        public void dsGetItemsByIdentFaction()
        {
            var ds = dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = dp.Parse(ds, Creator.DScreator);
            var parsedds = new DS(dsParse, config);

            var result = parsedds.GetItemsByIdent("faction");
            var expected = 21; //number of factions

            Assert.AreEqual(expected, result.Count); //check number of returned factions
        }
        [TestMethod]
        public void dsGetItemsByIdentCoreAttitudes()
        {
            var ds = dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = dp.Parse(ds, Creator.DScreator);
            var parsedds = new DS(dsParse, config);

            var result = parsedds.GetItemsByIdent("core_attitudes");
            var expected = 47; //number of ca

            Assert.AreEqual(expected, result.Count); //check number of returned ca
        }
        [TestMethod]
        public void dsDeleteByIdent()
        {
            var ds = dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = dp.Parse(ds, Creator.DScreator);
            var parsedds = new DS(dsParse, config);

            var settlements = parsedds.GetItemsByIdent("settlement");
            parsedds.DeleteValue(parsedds.data, "settlement");
            var result = parsedds.GetItemsByIdent("settlement");
            var expected = 0; //number of ca

            Assert.AreEqual(expected, result.Count); //check number of returned ca
        }
        [TestMethod]
        public void dsAddSettlementToRomansBrutii()
        {
            var ds = dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = dp.Parse(ds, Creator.DScreator);
            var parsedds = new DS(dsParse, config);

            var settlements = parsedds.GetItemsByIdent("settlement");
            var add = parsedds.InsertNewObjectByCriteria(parsedds.data, settlements[30], "faction\tromans_brutii,", "denari");
            var result = parsedds.GetItemsByCriteria("character", "settlement", "faction\tromans_brutii,");
            var expected = 3; //number of ca
     
            Assert.AreEqual(expected, result.Count); //check number of returned ca
        }
        [TestMethod]
        public void dsAddSettlementToScythia()
        {
            var ds = dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = dp.Parse(ds, Creator.DScreator);
            var parsedds = new DS(dsParse, config);
             
            var settlements = parsedds.GetItemsByIdent("settlement");
            var add = parsedds.InsertNewObjectByCriteria(parsedds.data, settlements[30], "faction\tscythia,", "denari");
            var result = parsedds.GetItemsByCriteria("character", "settlement", "faction\tscythia,");
            var expected = 5; //number of ca
            Assert.AreEqual(expected, result.Count); //check number of returned ca
        }
        [TestMethod]
        public void dsAddUnitToFlavius()
        {
            var ds = dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = dp.Parse(ds, Creator.DScreator);
            var parsedds = new DS(dsParse, config);
            var units = parsedds.GetItemsByCriteria("character", "unit", "faction\tromans_julii,", "character", "army");
            var add = parsedds.InsertNewObjectByCriteria(parsedds.data, units[1] , "faction\tromans_julii,", "character\tFlavius", "unit");
            var result = parsedds.GetItemsByCriteria("character", "unit", "character\tFlavius", "army");
            var expected = 6; //number of ca
            Assert.AreEqual(expected, result.Count); //check number of returned ca
        }

        [TestMethod]
        public void GetRegions()
        {
            var ds = dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = dp.Parse(ds, Creator.DScreator);
            var parsedds = new DS(dsParse, config);
            var regions = parsedds.GetItemsByCriteriaDepth(parsedds.data, "core_attitudes", "region", "settlement");

            var result = regions.Count;
            var expected = 96; //number of ca

            Assert.AreEqual(expected, result); //check number of returned ca
        }
        [TestMethod]
        public void GetFactionByRegion()
        {
            var ds = dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = dp.Parse(ds, Creator.DScreator);
            var parsedds = new DS(dsParse, config);
            string region = "Paionia";
            var faction = parsedds.GetFactionByRegion(region);
            
            var result = faction;
            var expected = "macedon"; //number of ca

            Assert.AreEqual(expected, result); //check number of returned ca
        }
    }
}
