namespace RTWLib_Tests.wrappers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.parsers.objects;

[TestClass]
public class Tests_dr
{
    [TestMethod]
    public void DRGetRegionDataLocusGepidae()
    {
        DR descr_regions = Instance.InstanceDR(TestHelper.Config, TestHelper.DR);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> result = descr_regions.GetNumberOfItems(8, "Locus_Gepidae");
        int expected = 8;
        Assert.AreEqual("Locus_Gepidae", ((BaseObj)result[0]).Value);
        Assert.AreEqual(expected, result.Count);
    }

    [TestMethod]
    public void DRGetRegionDataHibernia()
    {
        DR descr_regions = Instance.InstanceDR(TestHelper.Config, TestHelper.DR);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> result = descr_regions.GetNumberOfItems(8, "Hibernia");
        int expected = 8;
        Assert.AreEqual("Hibernia", ((BaseObj)result[0]).Value);
        Assert.AreEqual(expected, result.Count);
    }
    [TestMethod]
    public void DRGetRegionDataThebais()
    {
        DR descr_regions = Instance.InstanceDR(TestHelper.Config, TestHelper.DR);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> result = descr_regions.GetNumberOfItems(8, "Thebais");
        int expected = 8;
        Assert.AreEqual("Thebais", ((BaseObj)result[0]).Value);
        Assert.AreEqual(expected, result.Count);
    }

}
