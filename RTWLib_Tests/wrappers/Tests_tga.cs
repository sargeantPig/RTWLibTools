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
            TGA tga = new TGA();
            tga.Read("tgafile", RFH.CurrDirPath("resources/test.tga"));
            tga.Write("testtga.tga");
            Assert.AreEqual(2, tga.header.datatypecode);

        }
        [TestMethod]
        public void ReadCompressedTga()
        {
            TGA tga = new TGA();
            tga.Read("tgafile", RFH.CurrDirPath("resources/testcompressed.tga"));
            tga.Write("uncompressed.tga");
            Assert.AreEqual(10, tga.header.datatypecode);

        }
    }
}
