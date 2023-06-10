using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.parsers;
using RTWLibPlus.data;

namespace RTWLib_Tests.wrappers
{
    [TestClass]
    public class Tests_dr
    {
        DepthParse dp = new DepthParse();
        TWConfig config = TWConfig.LoadConfig(@"resources/remaster.json");
        [TestMethod]
        public void DRGetRegionDataLocusGepidae()
        {
            var smf = dp.ReadFile(RFH.CurrDirPath("resources", "descr_regions.txt"), false);
            var smfParse = dp.Parse(smf, Creator.DRcreator, '\t');
            var parsedsmf = new DR(smfParse, config);
            var result = parsedsmf.GetNumberOfItems(8, "Locus_Gepidae");
            var expected = 8;
            Assert.AreEqual("Locus_Gepidae", ((BaseObj)result[0]).Value);
            Assert.AreEqual(expected, result.Count);
        }

        [TestMethod]
        public void DRGetRegionDataHibernia()
        {
            var smf = dp.ReadFile(RFH.CurrDirPath("resources", "descr_regions.txt"), false);
            var smfParse = dp.Parse(smf, Creator.DRcreator, '\t');
            var parsedsmf = new DR(smfParse, config);
            var result = parsedsmf.GetNumberOfItems(8, "Hibernia");
            var expected = 8;
            Assert.AreEqual("Hibernia", ((BaseObj)result[0]).Value);
            Assert.AreEqual(expected, result.Count);
        }
        [TestMethod]
        public void DRGetRegionDataThebais()
        {
            var smf = dp.ReadFile(RFH.CurrDirPath("resources", "descr_regions.txt"), false);
            var smfParse = dp.Parse(smf, Creator.DRcreator, '\t');
            var parsedsmf = new DR(smfParse, config);
            var result = parsedsmf.GetNumberOfItems(8, "Thebais");
            var expected = 8;
            Assert.AreEqual("Thebais", ((BaseObj)result[0]).Value);
            Assert.AreEqual(expected, result.Count);
        }

    }
}
