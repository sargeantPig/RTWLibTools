namespace RTWLib_Tests.helper;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.helpers;

[TestClass]
public class Tests_Format
{

    [TestMethod]
    public void ReturnsCorrectWhiteSpace()
    {
        string result = Format.GetWhiteSpace("type", 20, ' ');
        string expected = "                ";
        Assert.AreEqual(expected, result);
    }
}
