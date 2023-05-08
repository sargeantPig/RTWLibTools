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

namespace RTWLib_Tests.wrappers
{
    [TestClass]
    public class Tests_citymap
    {
        [TestMethod]
        public void CityCoordinatesFetchedCorrectly() {
            TGA image = new TGA();
            image.Read("tgafile", RFH.CurrDirPath("resources", "map_regions.tga"));

            var drread = DepthParse.ReadFile(RFH.CurrDirPath("resources", "descr_regions.txt"), false);
            var drparse = DepthParse.Parse(drread, Creator.DRcreator, '\t');
            DR dr = new DR(drparse);

            CityMap cm = new CityMap(image, dr);

            Assert.IsTrue(cm.CityCoordinates.ContainsKey("Thebais"));
            Assert.IsTrue(cm.CityCoordinates.ContainsKey("Celtiberia"));
            Assert.IsTrue(cm.CityCoordinates.ContainsKey("Britannia_Inferior"));

        }


    }
}
