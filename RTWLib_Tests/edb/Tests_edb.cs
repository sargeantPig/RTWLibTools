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

namespace RTWLib_Tests.edb
{
    [TestClass]
    public class Tests_edb
    {
        [TestMethod]
        public void edbParse()
        {
            baseObj.AlwaysArrays = new string[2] {"plugins","upgrades"};
            baseObj.DoubleSpace = new string[2] { "construction", "cost" };
            baseObj.DoubleSpaceEnding = new string[1] { "levels" };
            baseObj.WhiteSpaceSwap = new string[2] { "requires", "temple" };
            var edb = TokenParse.ReadFile(Path.Combine("resources", "edbExample.txt"));
            var edbParse = DepthParse.Parse(edb);
            var parsedEdb = new EDB(edbParse);

            string result = parsedEdb.Output();
            var expected = TokenParse.ReadFileAsString(Path.Combine("resources", "edbExample.txt"));

            RTWFileHelper.Write("./result.txt", result);
            RTWFileHelper.Write("./expected.txt", expected);

            int rl = result.Length;
            int el = expected.Length;

            Assert.AreEqual(el, rl);
            Assert.AreEqual(expected, result);

        }
        [TestMethod]
        public void edbWholeFile()
        {
            baseObj.AlwaysArrays = new string[2] { "plugins", "upgrades" };
            baseObj.DoubleSpace = new string[2] { "construction", "cost" };
            baseObj.DoubleSpaceEnding = new string[1] { "levels" };
            baseObj.WhiteSpaceSwap = new string[2] { "requires", "temple" };
            var edb = TokenParse.ReadFile("./resources/export_descr_buildings.txt");
            var edbParse = DepthParse.Parse(edb);
            var parsedEdb = new EDB(edbParse);

            string result = parsedEdb.Output();
            var expected = TokenParse.ReadFileAsString("./resources/export_descr_buildings.txt");

            RTWFileHelper.Write("./result.txt", result);
            RTWFileHelper.Write("./expected.txt", expected);

            int rl = result.Length;
            int el = expected.Length;

            Assert.AreEqual(el, rl);
            Assert.AreEqual(expected, result);

        }

    }
}
