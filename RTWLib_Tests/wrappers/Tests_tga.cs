using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLib_Tests.wrappers
{
    [TestClass]
    public class Tests_tga
    {
        [TestMethod]
        public void ReadTga()
        {
            TGA tga = new TGA("tgafile", RFH.CurrDirPath("resources/test.tga"), "testtga.tga");
            tga.Output();
            Assert.AreEqual(2, tga.header.datatypecode);

        }
        [TestMethod]
        public void ReadCompressedTga()
        {
            TGA tga = new TGA("tgafile", RFH.CurrDirPath("resources/testcompressed.tga"), "uncompressed.tga");
            tga.Output();
            Assert.AreEqual(10, tga.header.datatypecode);

        }

        [TestMethod]
        public void ReadMapTga()
        {
            TGA tga = new TGA("tgafile", RFH.CurrDirPath("resources/map_regions.tga"), "map_regionsTest.tga");
            tga.Output();
            Assert.AreEqual(2, tga.header.datatypecode);

        }
    }
}
