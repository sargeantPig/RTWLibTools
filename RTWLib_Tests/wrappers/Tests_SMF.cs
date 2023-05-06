using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers;
using RTWLibPlus.parsers.objects;
using System.Collections.Generic;

namespace RTWLib_Tests.wrappers
{
    [TestClass]
    public class Tests_smf
    {
        [TestMethod]
        public void SMFCheckDataIsParsedDepth1()
        {
            var smf = DepthParse.ReadFile(RFH.CurrDirPath("resources", "descr_sm_factions.txt"), false);
            var smfParse = DepthParse.Parse(smf, Creator.BaseCreator, ':');
            var parsedsmf = new SMF(smfParse);
            var result = parsedsmf.GetKeyValueAtLocation(parsedsmf.data, 0, "romans_julii", "culture");
            var expected = new KeyValuePair<string, string>("culture", "roman");
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SMFCheckDataIsParsedDepth2()
        {
            var smf = DepthParse.ReadFile(RFH.CurrDirPath("resources", "descr_sm_factions.txt"), false);
            var smfParse = DepthParse.Parse(smf, Creator.BaseCreator, ':');
            var parsedsmf = new SMF(smfParse);
            var result = parsedsmf.GetKeyValueAtLocation(parsedsmf.data, 0, "germans", "colours", "primary");
            var expected = new KeyValuePair<string, string>("primary", "[88, 21, 38, ]");
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void SMFCheckDataIsParsedDepth3()
        {
            var smf = DepthParse.ReadFile(RFH.CurrDirPath("resources", "descr_sm_factions.txt"), false);
            var smfParse = DepthParse.Parse(smf, Creator.BaseCreator, ':');
            var parsedsmf = new SMF(smfParse);
            var result = parsedsmf.GetKeyValueAtLocation(parsedsmf.data, 0, "britons", "colours", "family tree", "selected line");
            var expected = new KeyValuePair<string, string>("selected line", "[255, 255, 255, ]");
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SMFCheckDataIsParsedDepth4()
        {
            var smf = DepthParse.ReadFile(RFH.CurrDirPath("resources", "descr_sm_factions.txt"), false);
            var smfParse = DepthParse.Parse(smf, Creator.BaseCreator, ':');
            var parsedsmf = new SMF(smfParse);

            var result = parsedsmf.GetKeyValueAtLocation(parsedsmf.data, 0, "thrace", "prefer naval invasions");
            var expected = new KeyValuePair<string, string>("prefer naval invasions", "false");

            Assert.AreEqual(expected, result);

        }

    }
}
