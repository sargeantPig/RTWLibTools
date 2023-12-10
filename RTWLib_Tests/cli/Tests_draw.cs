namespace RTWLib_Tests.cli;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLib_CLI.draw;
using RTWLibPlus.helpers;

[TestClass]
public class Tests_draw
{
    [TestMethod]
    public void BorderDrawnCorrectlyWidth1()
    {
        string result = "A Test".ApplyBorder('#', 1, 0);
        string expected =
            "########" + Format.UniversalNewLine() +
            "#A Test#" + Format.UniversalNewLine() +
            "########" + Format.UniversalNewLine();
        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public void BorderDrawnCorrectlyWidth1Padding1()
    {
        string result = "A Test".ApplyBorder('#', 1, 1);
        string expected =
              "##########" + Format.UniversalNewLine()
            + "#        #" + Format.UniversalNewLine()
            + "# A Test #" + Format.UniversalNewLine()
            + "#        #" + Format.UniversalNewLine()
            + "##########" + Format.UniversalNewLine();
        Assert.AreEqual(expected, result);
    }



}
