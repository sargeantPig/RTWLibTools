using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLib_CLI.draw;
using RTWLib_CLI;
using System;
using RTWLibPlus.helpers;

namespace RTWLib_Tests.cli
{
    [TestClass]
    public class Tests_draw
    {
        [TestMethod]
        public void BorderDrawnCorrectlyWidth1()
        {
            var result = "A Test".ApplyBorder('#', 1, 0);
            var expected = 
                "########" + Format.UniversalNewLine() +
                "#A Test#" + Format.UniversalNewLine() +
                "########" + Format.UniversalNewLine();
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void BorderDrawnCorrectlyWidth1Padding1()
        {
            var result = "A Test".ApplyBorder('#', 1, 1);
            var expected = 
                  "##########" + Format.UniversalNewLine() 
                + "#        #" + Format.UniversalNewLine() 
                + "# A Test #" + Format.UniversalNewLine() 
                + "#        #" + Format.UniversalNewLine() 
                + "##########" + Format.UniversalNewLine();
            Assert.AreEqual(expected, result);
        }



    }
}
