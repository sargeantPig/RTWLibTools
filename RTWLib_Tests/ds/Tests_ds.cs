using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLib_Tests.dummy;
using RTWLibPlus;
using System;
using System.Collections.Generic;
using System.Text;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers;
using RTWLibPlus.edb;
using RTWLibPlus.parsers.objects;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using System.IO;
using RTWLibPlus.ds;

namespace RTWLib_Tests.ds
{
    [TestClass]
    public class Tests_ds
    {
        [TestMethod]
        public void dsWholeFile()
        {       
            var edb = TokenParse.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
            var edbParse = DepthParse.Parse(edb, DSObj.creator);
            var parsedEdb = new DS(edbParse);

            string result = parsedEdb.Output();
            var expected = TokenParse.ReadFileAsString(RFH.CurrDirPath("resources", "descr_strat.txt"));

            RFH.Write("./dsresult.txt", result);
            RFH.Write("./dsexpected.txt", expected);

            int rl = result.Length;
            int el = expected.Length;

            Assert.AreEqual(el, rl);
            Assert.AreEqual(expected, result);

        }

    }
}
