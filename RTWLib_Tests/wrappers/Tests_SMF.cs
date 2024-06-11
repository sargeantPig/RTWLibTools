namespace RTWLib_Tests.wrappers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers;
using RTWLibPlus.parsers.objects;
using System.Collections.Generic;

[TestClass]
public class Tests_smf
{
    private readonly DepthParse dp = new();
    private readonly TWConfig config = TWConfig.LoadConfig(@"resources/remaster.json");

    [TestMethod]
    public void SMFCheckDataIsParsedDepth1()
    {
        string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_sm_factions.txt"), false);
        List<RTWLibPlus.interfaces.IBaseObj> smfParse = this.dp.Parse(smf, Creator.SMFcreator, ':');
        SMF parsedsmf = new(smfParse, this.config);
        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(parsedsmf.Data, 0, "romans_julii", "culture");
        KeyValuePair<string, string> expected = new("culture", "roman");
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SMFCheckDataIsParsedDepth2()
    {
        string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_sm_factions.txt"), false);
        List<RTWLibPlus.interfaces.IBaseObj> smfParse = this.dp.Parse(smf, Creator.SMFcreator, ':');
        SMF parsedsmf = new(smfParse, this.config);
        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(parsedsmf.Data, 0, "germans", "colours", "primary");
        KeyValuePair<string, string> expected = new("primary", "[88, 21, 38, ]");
        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public void SMFCheckDataIsParsedDepth3()
    {
        string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_sm_factions.txt"), false);
        List<RTWLibPlus.interfaces.IBaseObj> smfParse = this.dp.Parse(smf, Creator.SMFcreator, ':');
        SMF parsedsmf = new(smfParse, this.config);
        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(parsedsmf.Data, 0, "britons", "colours", "family tree", "selected line");
        KeyValuePair<string, string> expected = new("selected line", "[255, 255, 255, ]");
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SMFCheckDataIsParsedDepth4()
    {
        string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_sm_factions.txt"), false);
        List<RTWLibPlus.interfaces.IBaseObj> smfParse = this.dp.Parse(smf, Creator.SMFcreator, ':');
        SMF parsedsmf = new(smfParse, this.config);

        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(parsedsmf.Data, 0, "thrace", "prefer naval invasions");
        KeyValuePair<string, string> expected = new("prefer naval invasions", "false");

        Assert.AreEqual(expected, result);

    }
    [TestMethod]
    public void SMFGetFactions()
    {
        string[] smf = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_sm_factions.txt"), false);
        List<RTWLibPlus.interfaces.IBaseObj> smfParse = this.dp.Parse(smf, Creator.SMFcreator, ':');
        SMF parsedsmf = new(smfParse, this.config);

        int result = parsedsmf.GetFactions().Count;
        int expected = 21;

        Assert.AreEqual(expected, result);

    }

}
