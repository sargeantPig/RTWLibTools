namespace RTWLib_Tests.wrappers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers.objects;
using System.Collections.Generic;

[TestClass]
public class Tests_smf
{

    [TestMethod]
    public void SMFCheckDataIsParsedDepth1()
    {
        SMF smf = (SMF)RFH.CreateWrapper(Creator.SMFcreator, Creator.SMFWrapper, TestHelper.Config, ':', false, TestHelper.SMF);
        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(smf.Data, 0, "romans_julii", "culture");
        KeyValuePair<string, string> expected = new("culture", "roman");
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SMFCheckDataIsParsedDepth2()
    {
        SMF smf = (SMF)RFH.CreateWrapper(Creator.SMFcreator, Creator.SMFWrapper, TestHelper.Config, ':', false, TestHelper.SMF);
        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(smf.Data, 0, "germans", "colours", "primary");
        KeyValuePair<string, string> expected = new("primary", "[88, 21, 38, ]");
        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public void SMFCheckDataIsParsedDepth3()
    {
        SMF smf = (SMF)RFH.CreateWrapper(Creator.SMFcreator, Creator.SMFWrapper, TestHelper.Config, ':', false, TestHelper.SMF);
        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(smf.Data, 0, "britons", "colours", "family tree", "selected line");
        KeyValuePair<string, string> expected = new("selected line", "[255, 255, 255, ]");
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SMFCheckDataIsParsedDepth4()
    {
        SMF smf = (SMF)RFH.CreateWrapper(Creator.SMFcreator, Creator.SMFWrapper, TestHelper.Config, ':', false, TestHelper.SMF);
        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(smf.Data, 0, "thrace", "prefer naval invasions");
        KeyValuePair<string, string> expected = new("prefer naval invasions", "false");

        Assert.AreEqual(expected, result);

    }
    [TestMethod]
    public void SMFGetFactions()
    {
        SMF smf = (SMF)RFH.CreateWrapper(Creator.SMFcreator, Creator.SMFWrapper, TestHelper.Config, ':', false, TestHelper.SMF);

        int result = smf.GetFactions().Count;
        int expected = 21;

        Assert.AreEqual(expected, result);

    }

}
