namespace RTWLib_Tests.Modifiers;

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.Modifiers;

[TestClass]
public class TestsDR
{
    private readonly DepthParse dp = new();
    private readonly TWConfig config = TWConfig.LoadConfig(@"resources/remaster.json");

    [TestMethod]
    public void GetMissingRegions()
    {
        string[] dr = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_regions.txt"), false);
        List<IBaseObj> smfParse = this.dp.Parse(dr, Creator.DRcreator, '\t');
        DR parseddr = new(smfParse, this.config);
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        List<IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);
        List<IBaseObj> settlements = parsedds.GetItemsByIdent("settlement").DeepCopy();
        List<string> missing = DRModifier.GetMissingRegionNames(settlements, parseddr);

        int result = settlements.Count + missing.Count;
        Assert.AreEqual(parseddr.Regions.Count, result);
    }

}
