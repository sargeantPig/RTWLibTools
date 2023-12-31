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
public class Tests_ds
{
    private readonly DepthParse dp = new();
    private readonly TWConfig config = TWConfig.LoadConfig(@"resources/remaster.json");
    public void GetFactionByRegion()
    {
        string[] ds = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_strat.txt"), false);
        List<IBaseObj> dsParse = this.dp.Parse(ds, Creator.DScreator);
        DS parsedds = new(dsParse, this.config);
        List<IBaseObj> settlements = parsedds.GetItemsByIdent("settlement").DeepCopy();
        IBaseObj modifiedSettlement = StratModifier.CreateSettlement(settlements[0], "test_name");

        Assert.AreEqual("test_name", modifiedSettlement.Find("region"));
    }
}
