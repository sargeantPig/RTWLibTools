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
using RTWLibPlus.interfaces;

namespace RTWLib_Tests.edb
{
    [TestClass]
    public class Tests_edb
    {
        [TestMethod]
        public void edbParse()
        {
            EDBObj.AlwaysArrays = new string[2] {"plugins","upgrades"};
            EDBObj.DoubleSpace = new string[2] { "construction", "cost" };
            EDBObj.DoubleSpaceEnding = new string[1] { "levels" };
            EDBObj.WhiteSpaceSwap = new string[2] { "requires", "temple" };
            var edb = TokenParse.ReadFile(Path.Combine("resources", "edbExample.txt"));
            var edbParse = DepthParse.Parse(edb, EDBObj.creator);
            var parsedEdb = new EDB(edbParse);

            string result = parsedEdb.Output();
            var expected = TokenParse.ReadFileAsString(RFH.CurrDirPath("resources", "edbExample.txt"));

            RFH.Write("./result.txt", result);
            RFH.Write("./expected.txt", expected);

            int rl = result.Length;
            int el = expected.Length;

            Assert.AreEqual(el, rl);
            Assert.AreEqual(expected, result);

        }
        [TestMethod]
        public void edbWholeFile()
        {
            EDBObj.AlwaysArrays = new string[2] { "plugins", "upgrades" };
            EDBObj.DoubleSpace = new string[2] { "construction", "cost" };
            EDBObj.DoubleSpaceEnding = new string[1] { "levels" };
            EDBObj.WhiteSpaceSwap = new string[2] { "requires", "temple" };
            var edb = TokenParse.ReadFile(RFH.CurrDirPath("resources", "export_descr_buildings.txt"));
            var edbParse = DepthParse.Parse(edb, EDBObj.creator);
            var parsedEdb = new EDB(edbParse);

            string result = parsedEdb.Output();
            var expected = TokenParse.ReadFileAsString(RFH.CurrDirPath("resources", "export_descr_buildings.txt"));

            RFH.Write("./result.txt", result);
            RFH.Write("./expected.txt", expected);

            int rl = result.Length;
            int el = expected.Length;

            Assert.AreEqual(el, rl);
            Assert.AreEqual(expected, result);

        }

    }
}
