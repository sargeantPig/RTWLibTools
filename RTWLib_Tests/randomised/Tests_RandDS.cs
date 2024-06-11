namespace RTWLib_Tests.randomised;

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.randomiser;

[TestClass]
public class Tests_RandDS
{
    private readonly TWConfig config = TWConfig.LoadConfig(@"resources/remaster.json");
    private readonly RandWrap rand = new("test");
    [TestMethod]
    public void UnitsAreRecruitable()
    {
        List<IBaseObj> dsParse = RFH.ParseFile(Creator.DScreator, ' ', false, "resources", "descr_strat.txt");
        DS ds = new(dsParse, this.config);
        List<IBaseObj> parse = RFH.ParseFile(Creator.EDUcreator, ' ', false, "resources", "export_descr_unit.txt");
        EDU edu = new(parse, this.config);
        edu.PrepareEDU();
        List<IBaseObj> smfParse = RFH.ParseFile(Creator.SMFcreator, ':', false, "resources", "descr_sm_factions.txt");
        SMF smf = new(smfParse, this.config);
        List<string> beforeUnits = edu.GetUnitsFromFaction("romans_julii");
        RandEDU.RandomiseOwnership(edu, this.rand, smf);
        RandDS.SwitchUnitsToRecruitable(edu, ds);
        List<IBaseObj> units = ds.GetItemsByCriteria("character", "unit", "faction\tromans_julii,", "character", "army");
        List<string> eduUnits = edu.GetUnitsFromFaction("romans_julii");
        //RFH.Write("eddu-test.txt", edu.Output());
        foreach (IBaseObj unit in units)
        {
            string name = DS.GetUnitName(unit);
            bool has = eduUnits.Contains(name);

            Assert.AreEqual(true, has);
        }
    }
}
