﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLib_Tests.dummy;
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
        [TestMethod]
        public void eduWithDepthParser()
        {
            var edu = DepthParse.ReadFile(RFH.CurrDirPath("resources", "export_descr_unit.txt"), false);
            var eduParse = DepthParse.Parse(edu, Creator.EDUcreator);
            var parsedds = new EDU(eduParse);

            string result = parsedds.Output();
            var expected = DepthParse.ReadFileAsString(RFH.CurrDirPath("resources", "export_descr_unit.txt"));

            int rl = result.Length;
            int el = expected.Length;

            Assert.AreEqual(el, rl);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void eduGetValueByCriteria()
        {
            var edu = DepthParse.ReadFile(RFH.CurrDirPath("resources", "export_descr_unit.txt"), false);
            var eduParse = DepthParse.Parse(edu, Creator.EDUcreator);
            var parsedds = new EDU(eduParse);
            var result = parsedds.GetKeyValueAtLocation(parsedds.data, 0, "roman_hastati", "ownership");
            var expected = new KeyValuePair<string, string>("ownership", "roman"); //number of ca
  
            Assert.AreEqual(expected, result); //check number of returned ca
        }

        [TestMethod]
        public void eduModifyOwnership()
        {
            var parsedds = new EDU(RFH.ParseFile(Creator.EDUcreator, ' ', false, "resources", "export_descr_unit.txt") );

            var change = parsedds.ModifyValue(parsedds.data, "roman", 0, false, "carthaginian_generals_cavalry_early", "ownership");
            var result = parsedds.GetKeyValueAtLocation(parsedds.data, 0, "carthaginian_generals_cavalry_early", "ownership");
            var expected = new KeyValuePair<string, string>("ownership", "roman");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(true, change);
        }

        [TestMethod]
        public void eduModifyBulkModifyOwnership()
        {
            var edu = new EDU(RFH.ParseFile(Creator.EDUcreator, ' ', false, "resources", "export_descr_unit.txt"));
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
    }
}