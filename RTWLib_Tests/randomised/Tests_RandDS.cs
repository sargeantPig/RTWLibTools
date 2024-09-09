namespace RTWLib_Tests.randomised;

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.dataWrappers.TGA;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.map;
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
        DS ds = (DS)RFH.CreateWrapper(Creator.DScreator, Creator.DSWrapper, this.config, ' ', false, "resources", "descr_strat.txt");
        EDU edu = (EDU)RFH.CreateWrapper(Creator.EDUcreator, Creator.EDUWrapper, this.config, ' ', false, "resources", "export_descr_unit.txt");
        SMF smf = (SMF)RFH.CreateWrapper(Creator.SMFcreator, Creator.SMFWrapper, this.config, ':', false, "resources", "descr_sm_factions.txt");
        edu.PrepareEDU();

        List<string> beforeUnits = edu.GetUnitsFromFaction("romans_julii", []);
        RandEDU.RandomiseOwnership(edu, this.rand, smf);
        RandDS.SwitchUnitsToRecruitable(edu, ds, this.rand);
        List<IBaseObj> units = ds.GetItemsByCriteria("character", "unit", "faction\tromans_julii,", "character", "army");
        List<string> eduUnits = edu.GetUnitsFromFaction("romans_julii", []);
        //RFH.Write("eddu-test.txt", edu.Output());
        foreach (IBaseObj unit in units)
        {
            string name = DS.GetUnitName(unit);
            bool has = eduUnits.Contains(name);

            Assert.AreEqual(true, has);
        }
    }

    [TestMethod]
    public void RelationsAreRandom()
    {
        DS ds = (DS)RFH.CreateWrapper(Creator.DScreator, Creator.DSWrapper, this.config, ' ', false, "resources", "descr_strat.txt");
        SMF smf = (SMF)RFH.CreateWrapper(Creator.SMFcreator, Creator.SMFWrapper, this.config, ':', false, "resources", "descr_sm_factions.txt");
        TGA image = new("tgafile", RFH.CurrDirPath("resources", "map_regions.tga"), "");
        DR dr = (DR)RFH.CreateWrapper(Creator.DRcreator, Creator.DRWrapper, this.config, '\t', false, "resources", "descr_regions.txt");

        CityMap cm = new(image, dr);
        List<IBaseObj> before = ds.GetItemsByIdent("core_attitudes").DeepCopy();
        RandDS.RandRelations(ds, smf, cm, 30);
        List<IBaseObj> after = ds.GetItemsByIdent("core_attitudes").DeepCopy();

        Assert.IsTrue(before.Count < after.Count);
    }
}
