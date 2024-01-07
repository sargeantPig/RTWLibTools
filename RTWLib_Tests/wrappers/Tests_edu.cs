namespace RTWLib_Tests.wrappers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.data;
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
    private readonly DepthParse dp = new();
    private readonly TWConfig config = TWConfig.LoadConfig(@"resources/remaster.json");
    [TestMethod]
    public void EduWithDepthParser()
    {
        string[] edu = this.dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_unit.txt"), false);
        List<IBaseObj> eduParse = this.dp.Parse(edu, Creator.EDUcreator);
        EDU parsedds = new(eduParse, this.config);

        string result = parsedds.Output();
        string expected = this.dp.ReadFileAsString(RFH.CurrDirPath("resources", "export_descr_unit.txt"));

        int rl = result.Length;
        int el = expected.Length;

        Assert.AreEqual(el, rl);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void EduGetValueByCriteria()
    {
        string[] edu = this.dp.ReadFile(RFH.CurrDirPath("resources", "export_descr_unit.txt"), false);
        List<IBaseObj> eduParse = this.dp.Parse(edu, Creator.EDUcreator);
        EDU parsedds = new(eduParse, this.config);
        KeyValuePair<string, string> result = parsedds.GetKeyValueAtLocation(parsedds.Data, 0, "roman_hastati", "ownership");
        KeyValuePair<string, string> expected = new("ownership", "roman"); //number of ca

        Assert.AreEqual(expected, result); //check number of returned ca
    }

    [TestMethod]
    public void EduModifyOwnership()
    {
        List<IBaseObj> parse = RFH.ParseFile(Creator.EDUcreator, ' ', false, "resources", "export_descr_unit.txt");
        EDU parsedds = new(parse, this.config);

        bool change = parsedds.ModifyValue(parsedds.Data, "roman", 0, false, "carthaginian_generals_cavalry_early", "ownership");
        KeyValuePair<string, string> result = parsedds.GetKeyValueAtLocation(parsedds.Data, 0, "carthaginian_generals_cavalry_early", "ownership");
        KeyValuePair<string, string> expected = new("ownership", "roman");

        Assert.AreEqual(expected, result);
        Assert.AreEqual(true, change);
    }

    [TestMethod]
    public void EduModifyBulkModifyOwnership()
    {
        List<IBaseObj> parse = RFH.ParseFile(Creator.EDUcreator, ' ', false, "resources", "export_descr_unit.txt");
        EDU edu = new(parse, this.config);
        List<IBaseObj> ownerships = edu.GetItemsByIdent("ownership");

        foreach (EDUObj o in ownerships.Cast<EDUObj>())
        {
            o.Value = "roman";
        }

        KeyValuePair<string, string> result1 = edu.GetKeyValueAtLocation(edu.Data, 0, "carthaginian_generals_cavalry_early", "ownership");
        KeyValuePair<string, string> expected1 = new("ownership", "roman");

        KeyValuePair<string, string> result2 = edu.GetKeyValueAtLocation(edu.Data, 0, "roman_arcani", "ownership");
        KeyValuePair<string, string> expected2 = new("ownership", "roman");

        Assert.AreEqual(expected1, result1);
        Assert.AreEqual(expected2, result2);
    }

    [TestMethod]
    public void EduRemoveAttributeGeneral()
    {
        List<IBaseObj> parse = RFH.ParseFile(Creator.EDUcreator, ' ', false, "resources", "export_descr_unit.txt");
        EDU parsedds = new(parse, this.config);
        parsedds.RemoveAttributesAll("general_unit", "general_unit_upgrade \"marian_reforms\"");
        KeyValuePair<string, string> result = parsedds.GetKeyValueAtLocation(parsedds.Data, 0, "barb_chieftain_cavalry_german", "attributes");
        KeyValuePair<string, string> expected = new("attributes", "sea_faring, hide_forest, hardy");
        List<IBaseObj> attr = parsedds.GetItemsByIdent("attributes");

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void EduRemoveRemasterStatBlocks()
    {
        List<IBaseObj> parse = RFH.ParseFile(Creator.EDUcreator, ' ', false, "resources", "export_descr_unit.txt");
        EDU parsedds = new(parse, this.config);
        parsedds.PrepareEDU();
        List<IBaseObj> result = parsedds.GetItemsByIdent("rebalance_statblock");
        int expected = 0;

        string file = parsedds.Output();

        Assert.AreEqual(expected, result.Count);
    }
    [TestMethod]
    public void EduGetUnitsFromFaction()
    {
        List<IBaseObj> parse = RFH.ParseFile(Creator.EDUcreator, ' ', false, "resources", "export_descr_unit.txt");
        EDU parsedds = new(parse, this.config);
        List<string> units = parsedds.GetUnitsFromFaction("romans_julii");
        Assert.AreEqual(27, units.Count);

    }

    // [TestMethod]
    // public void eduUnitWrapper()
    // {
    //     var parse = RFH.ParseFile(Creator.EDUcreator, ' ', false, "resources", "export_descr_unit.txt");
    //     var parsedds = new EDU(parse, config);
    //     parsedds.PrepareEDU();

    //     UnitsWrapper uw = new UnitsWrapper(parsedds);

    //     var result = 0;
    //     var expected = 1;


    //     Assert.AreEqual(expected, result);
    // }
}
