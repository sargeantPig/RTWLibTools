namespace RTWLib_Tests.wrappers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.interfaces;
using System.Collections.Generic;
using System.Linq;

[TestClass]
public class Tests_edu
{
    [TestMethod]
    public void EduWithDepthParser()
    {
        EDU edu = Instance.InstanceEDU(TestHelper.Config, TestHelper.EDU);

        string result = edu.Output();
        string expected = DepthParse.ReadFileAsString(RFH.CurrDirPath(TestHelper.EDU));

        int rl = result.Length;
        int el = expected.Length;

        Assert.AreEqual(el, rl);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void EduGetValueByCriteria()
    {
        EDU edu = Instance.InstanceEDU(TestHelper.Config, TestHelper.EDU);
        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(edu.Data, 0, "roman_hastati", "ownership");
        KeyValuePair<string, string> expected = new("ownership", "roman"); //number of ca

        Assert.AreEqual(expected, result); //check number of returned ca
    }

    [TestMethod]
    public void EduModifyOwnership()
    {
        EDU edu = Instance.InstanceEDU(TestHelper.Config, TestHelper.EDU);
        bool change = BaseWrapper.ModifyValue(edu.Data, "roman", 0, false, "carthaginian_generals_cavalry_early", "ownership");
        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(edu.Data, 0, "carthaginian_generals_cavalry_early", "ownership");
        KeyValuePair<string, string> expected = new("ownership", "roman");

        Assert.AreEqual(expected, result);
        Assert.AreEqual(true, change);
    }

    [TestMethod]
    public void EduModifyBulkModifyOwnership()
    {
        EDU edu = Instance.InstanceEDU(TestHelper.Config, TestHelper.EDU);
        List<IBaseObj> ownerships = edu.GetItemsByIdent("ownership");

        foreach (EDUObj o in ownerships.Cast<EDUObj>())
        {
            o.Value = "roman";
        }

        KeyValuePair<string, string> result1 = BaseWrapper.GetKeyValueAtLocation(edu.Data, 0, "carthaginian_generals_cavalry_early", "ownership");
        KeyValuePair<string, string> expected1 = new("ownership", "roman");

        KeyValuePair<string, string> result2 = BaseWrapper.GetKeyValueAtLocation(edu.Data, 0, "roman_arcani", "ownership");
        KeyValuePair<string, string> expected2 = new("ownership", "roman");

        Assert.AreEqual(expected1, result1);
        Assert.AreEqual(expected2, result2);
    }

    [TestMethod]
    public void EduRemoveAttributeGeneral()
    {
        EDU edu = Instance.InstanceEDU(TestHelper.Config, TestHelper.EDU);
        edu.RemoveAttributesAll("general_unit", "general_unit_upgrade \"marian_reforms\"");
        KeyValuePair<string, string> result = BaseWrapper.GetKeyValueAtLocation(edu.Data, 0, "barb_chieftain_cavalry_german", "attributes");
        KeyValuePair<string, string> expected = new("attributes", "sea_faring, hide_forest, hardy");
        List<IBaseObj> attr = edu.GetItemsByIdent("attributes");

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void EduRemoveRemasterStatBlocks()
    {
        EDU edu = Instance.InstanceEDU(TestHelper.Config, TestHelper.EDU);
        edu.PrepareEDU();
        List<IBaseObj> result = edu.GetItemsByIdent("rebalance_statblock");
        int expected = 0;

        string file = edu.Output();

        Assert.AreEqual(expected, result.Count);
    }
    [TestMethod]
    public void EduGetUnitsFromFaction()
    {
        EDU edu = Instance.InstanceEDU(TestHelper.Config, TestHelper.EDU);
        List<string> units = edu.GetUnitsFromFaction("romans_julii", []);
        Assert.AreEqual(27, units.Count);

    }

    [TestMethod]
    public void EduValuesInCorrectOrder()
    {
        EDU edu = Instance.InstanceEDU(TestHelper.Config, TestHelper.EDU);
        List<IBaseObj> names = edu.GetItemsByIdent("type");
        List<IBaseObj> attributes = edu.GetItemsByIdent("attributes");
        List<IBaseObj> ownerships = edu.GetItemsByIdent("ownership");
        List<IBaseObj> category = edu.GetItemsByIdent("category");

        Assert.AreEqual(names[0].Value, "barb peasant briton");
        Assert.AreEqual(attributes[0].Value, "sea_faring, hide_improved_forest");
        Assert.AreEqual(ownerships[0].Value, "britons");
        Assert.AreEqual(category[0].Value, "infantry");

        Assert.AreEqual(names[^1].Value, "egyptian female peasant");
        Assert.AreEqual(attributes[^1].Value, "sea_faring, hide_forest, can_sap, no_custom");
        Assert.AreEqual(ownerships[^1].Value, "egyptian");
        Assert.AreEqual(category[^1].Value, "non_combatant");
    }

    // [TestMethod]
    // public void eduUnitWrapper()
    // {
    //     var parse = RFH.ParseFile(Creator.EDUcreator, ' ', false, TestHelper.EDU);
    //     var parsedds = new EDU(parse, config);
    //     parsedds.PrepareEDU();

    //     UnitsWrapper uw = new UnitsWrapper(parsedds);

    //     var result = 0;
    //     var expected = 1;


    //     Assert.AreEqual(expected, result);
    // }
}
