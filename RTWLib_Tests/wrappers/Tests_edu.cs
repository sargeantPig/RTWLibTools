using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLib_Tests.dummy;
using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.edu;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace RTWLib_Tests.wrappers
{
    [TestClass]
    public class Tests_edu
    {
        DepthParse dp = new DepthParse();
        RemasterRome config = RemasterRome.LoadConfig(@"resources/remaster.json");
        [TestMethod]
        public void eduWithDepthParser()
        {
            var edu = dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_unit.txt"), false);
            var eduParse = dp.Parse(edu, Creator.EDUcreator);
            var parsedds = new EDU(eduParse, config);

            string result = parsedds.Output();
            var expected = dp.ReadFileAsString(RFH.CurrDirPath("resources", "export_descr_unit.txt"));

            int rl = result.Length;
            int el = expected.Length;

            Assert.AreEqual(el, rl);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void eduGetValueByCriteria()
        {
            var edu = dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_unit.txt"), false);
            var eduParse = dp.Parse(edu, Creator.EDUcreator);
            var parsedds = new EDU(eduParse, config);
            var result = parsedds.GetKeyValueAtLocation(parsedds.data, 0, "roman_hastati", "ownership");
            var expected = new KeyValuePair<string, string>("ownership", "roman"); //number of ca
  
            Assert.AreEqual(expected, result); //check number of returned ca
        }

        [TestMethod]
        public void eduModifyOwnership()
        {
            var parse = RFH.ParseFile(Creator.EDUcreator, ' ', false, "resources", "export_descr_unit.txt");
            var parsedds = new EDU(parse, config);

            var change = parsedds.ModifyValue(parsedds.data, "roman", 0, false, "carthaginian_generals_cavalry_early", "ownership");
            var result = parsedds.GetKeyValueAtLocation(parsedds.data, 0, "carthaginian_generals_cavalry_early", "ownership");
            var expected = new KeyValuePair<string, string>("ownership", "roman");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(true, change);
        }

        [TestMethod]
        public void eduModifyBulkModifyOwnership()
        {
            var parse = RFH.ParseFile(Creator.EDUcreator, ' ', false, "resources", "export_descr_unit.txt");
            var edu = new EDU(parse, config);
            var ownerships = edu.GetItemsByIdent("ownership");

            foreach (EDUObj o in ownerships)
            {
                o.Value = "roman";
            }

            var result1 = edu.GetKeyValueAtLocation(edu.data, 0, "carthaginian_generals_cavalry_early", "ownership");
            var expected1 = new KeyValuePair<string, string>("ownership", "roman");

            var result2 = edu.GetKeyValueAtLocation(edu.data, 0, "roman_arcani", "ownership");
            var expected2 = new KeyValuePair<string, string>("ownership", "roman");

            Assert.AreEqual(expected1, result1);
            Assert.AreEqual(expected2, result2);
        }

        [TestMethod]
        public void eduRemoveAttributeGeneral()
        {
            var parse = RFH.ParseFile(Creator.EDUcreator, ' ', false, "resources", "export_descr_unit.txt");
            var parsedds = new EDU(parse, config);
            parsedds.RemoveAttributesAll("general_unit", "general_unit_upgrade \"marian_reforms\"");
            var result = parsedds.GetKeyValueAtLocation(parsedds.data, 0, "barb_chieftain_cavalry_german", "attributes");
            var expected = new KeyValuePair<string, string>("attributes", "sea_faring, hide_forest, hardy");

            Assert.AreEqual(expected, result);
        }
    }
}
