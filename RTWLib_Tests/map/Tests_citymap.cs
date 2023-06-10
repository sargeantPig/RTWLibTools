using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.map;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.parsers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RTWLibPlus.data;

namespace RTWLib_Tests.map
{
    [TestClass]
    public class Tests_citymap
    {
        DepthParse dp = new DepthParse();
        TWConfig config = TWConfig.LoadConfig(@"resources/remaster.json");
        [TestMethod]
        public void CityCoordinatesFetchedCorrectly()
        {
            TGA image = new TGA("tgafile", RFH.CurrDirPath("resources", "map_regions.tga"), "");

            var drread = dp.ReadFile(RFH.CurrDirPath("resources", "descr_regions.txt"), false);
            var drparse = dp.Parse(drread, Creator.DRcreator, '\t');
            DR dr = new DR(drparse, config);

            CityMap cm = new CityMap(image, dr);

            Assert.IsTrue(cm.CityCoordinates.ContainsKey("Thebais"));
            Assert.IsTrue(cm.CityCoordinates.ContainsKey("Celtiberia"));
            Assert.IsTrue(cm.CityCoordinates.ContainsKey("Britannia_Inferior"));

        }


    }
}
