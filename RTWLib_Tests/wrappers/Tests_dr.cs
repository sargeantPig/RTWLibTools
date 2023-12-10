namespace RTWLib_Tests.wrappers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.parsers;
using RTWLibPlus.data;

[TestClass]
public class Tests_dr
{
    private readonly DepthParse dp = new();
    private readonly TWConfig config = TWConfig.LoadConfig(@"resources/remaster.json");
    [TestMethod]
    public void DRGetRegionDataLocusGepidae()
    {
        string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_regions.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> smfParse = this.dp.Parse(smf, Creator.DRcreator, '\t');
        DR parsedsmf = new(smfParse, this.config);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> result = parsedsmf.GetNumberOfItems(8, "Locus_Gepidae");
        int expected = 8;
        Assert.AreEqual("Locus_Gepidae", ((BaseObj)result[0]).Value);
        Assert.AreEqual(expected, result.Count);
    }

    [TestMethod]
    public void DRGetRegionDataHibernia()
    {
        string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_regions.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> smfParse = this.dp.Parse(smf, Creator.DRcreator, '\t');
        DR parsedsmf = new(smfParse, this.config);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> result = parsedsmf.GetNumberOfItems(8, "Hibernia");
        int expected = 8;
        Assert.AreEqual("Hibernia", ((BaseObj)result[0]).Value);
        Assert.AreEqual(expected, result.Count);
    }
    [TestMethod]
    public void DRGetRegionDataThebais()
    {
        string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_regions.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> smfParse = this.dp.Parse(smf, Creator.DRcreator, '\t');
        DR parsedsmf = new(smfParse, this.config);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> result = parsedsmf.GetNumberOfItems(8, "Thebais");
        int expected = 8;
        Assert.AreEqual("Thebais", ((BaseObj)result[0]).Value);
        Assert.AreEqual(expected, result.Count);
    }

}
