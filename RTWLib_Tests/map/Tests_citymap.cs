namespace RTWLib_Tests.map;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.map;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.parsers;
using RTWLibPlus.data;
using RTWLibPlus.dataWrappers.TGA;

[TestClass]
public class Tests_citymap
{
    private readonly DepthParse dp = new();
    private readonly TWConfig config = TWConfig.LoadConfig(@"resources/remaster.json");
    [TestMethod]
    public void CityCoordinatesFetchedCorrectly()
    {
        TGA image = new("tgafile", RFH.CurrDirPath("resources", "map_regions.tga"), "");

        string[] drread = this.dp.ReadFile(RFH.CurrDirPath("resources", "descr_regions.txt"), false);
        DR dr = (DR)RFH.CreateWrapper(Creator.DRcreator, Creator.DRWrapper, this.config, '\t', false, "resources", "descr_regions.txt");

        CityMap cm = new(image, dr);

        Assert.IsTrue(cm.CityCoordinates.ContainsKey("Thebais"));
        Assert.IsTrue(cm.CityCoordinates.ContainsKey("Celtiberia"));
        Assert.IsTrue(cm.CityCoordinates.ContainsKey("Britannia_Inferior"));
    }


}
