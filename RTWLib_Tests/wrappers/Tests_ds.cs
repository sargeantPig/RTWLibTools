﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers;
using RTWLibPlus.parsers.objects;

namespace RTWLib_Tests.wrappers
{
    [TestClass]
    public class Tests_ds
    {
        [TestMethod]
        public void dsWholeFile()
        {
            var ds = TokenParse.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = DepthParse.Parse(ds, Creator.DScreator);
            var parsedds = new DS(dsParse);

            string result = parsedds.Output();
            var expected = TokenParse.ReadFileAsString(RFH.CurrDirPath("resources", "descr_strat.txt"));

            RFH.Write("./dsresult.txt", result);
            RFH.Write("./dsexpected.txt", expected);

            int rl = result.Length;
            int el = expected.Length;

            Assert.AreEqual(el, rl);
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void dsGetItemsByIdentSettlements()
        {
            var ds = TokenParse.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = DepthParse.Parse(ds, Creator.DScreator);
            var parsedds = new DS(dsParse);

            var result = parsedds.GetItemsByIdent("settlement");
            var expected = 96; //number of settlements

            Assert.AreEqual(expected, result.Count); //check number of returned settlements
        }
        [TestMethod]
        public void dsGetItemsByIdentResource()
        {
            var ds = TokenParse.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = DepthParse.Parse(ds, Creator.DScreator);
            var parsedds = new DS(dsParse);

            var result = parsedds.GetItemsByIdent("resource");
            var expected = 300; //number of settlements

            Assert.AreEqual(expected, result.Count); //check number of returned settlements
        }

        /*[TestMethod]
        public void dsWholeFile()
        {
            var ds = TokenParse.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var dsParse = DepthParse.Parse(ds, DSObj.creator);
            var parsedds = new DS(dsParse);

            string result =
            var expected;


            Assert.AreEqual(el, rl);
            Assert.AreEqual(expected, result);
        }*/

    }
}
