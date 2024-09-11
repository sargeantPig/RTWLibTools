namespace RTWLib_Tests.Modifiers;

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.Modifiers;

[TestClass]
public class TestsDR
{
    [TestMethod]
    public void GetMissingRegions()
    {
        DR descr_regions = Instance.InstanceDR(TestHelper.Config, TestHelper.DR);
        DS descr_strat = Instance.InstanceDS(TestHelper.Config, TestHelper.DS);
        List<IBaseObj> settlements = descr_strat.GetItemsByIdent("settlement").DeepCopy();
        List<string> missing = DRModifier.GetMissingRegionNames(settlements, descr_regions);
        int result = settlements.Count + missing.Count;
        Assert.AreEqual(descr_regions.Regions.Count, result);
    }

}
