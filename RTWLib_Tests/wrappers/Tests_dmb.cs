namespace RTWLib_Tests.wrappers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.parsers;
using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.dataWrappers;
using System.Linq;

[TestClass]
public class Tests_dmb
{
    private readonly DepthParse dp = new();
    private readonly TWConfig config = TWConfig.LoadConfig(@"resources/remaster.json");

    // [TestMethod]
    // public void ReadDMB()
    // {
    //     string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_model_battle.txt"), false);
    //     System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> smfParse = this.dp.Parse(smf, Creator.DMBcreator, ' ');
    //     DMB dmb = new(smfParse, this.config);

    //     // measure by line not characters. DMB is too annoying to get the exact same formatting on
    //     string[] result = dmb.Output().Split('\n');
    //     string[] expected = this.dp.ReadFileAsString(RFH.CurrDirPath("resources", "descr_model_battle.txt")).Split('\n');
    //     string elast = expected[^3];
    //     string eresult = result[^3];
    //     Assert.AreEqual(expected[^3], result[^3]);
    // }

    [TestMethod]
    public void FallbackForAllTextures()
    {
        string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_model_battle.txt"), false);
        System.Collections.Generic.List<RTWLibPlus.interfaces.IBaseObj> smfParse = this.dp.Parse(smf, Creator.DMBcreator, ' ');
        DMB dmb = new(smfParse, this.config);
        int textureCountOrig = dmb.GetItemsByIdent("texture").Count;
        int typeCountOrig = dmb.GetItemsByIdent("type").Count;
        dmb.AddFallBacksForAllTypes();
        int textureCountNew = dmb.GetItemsByIdent("texture").Count;

        int diff = textureCountNew - textureCountOrig;
        RFH.Write("test_dmb_result.txt", dmb.Output());

        Assert.AreEqual(typeCountOrig, diff);

    }

}
