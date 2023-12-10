namespace RTWLib_Tests.wrappers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.dataWrappers.TGA;
using RTWLibPlus.helpers;

[TestClass]
public class Tests_tga
{
    [TestMethod]
    public void ReadTga()
    {
        TGA tga = new("tgafile", RFH.CurrDirPath("resources/test.tga"), "testtga.tga");
        tga.Output();
        Assert.AreEqual(2, tga.RefHeader.Datatypecode);

    }
    [TestMethod]
    public void ReadCompressedTga()
    {
        TGA tga = new("tgafile", RFH.CurrDirPath("resources/testcompressed.tga"), "uncompressed.tga");
        tga.Output();
        Assert.AreEqual(10, tga.RefHeader.Datatypecode);

    }

    [TestMethod]
    public void ReadMapTga()
    {
        TGA tga = new("tgafile", RFH.CurrDirPath("resources/map_regions.tga"), "map_regionsTest.tga");
        tga.Output();
        Assert.AreEqual(2, tga.RefHeader.Datatypecode);

    }
}
