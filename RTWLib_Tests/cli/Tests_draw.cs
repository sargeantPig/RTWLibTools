using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLib_CLI.draw;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLib_Tests.cli
{
    [TestClass]
    public class Tests_draw
    {
        [TestMethod]
        public void BorderDrawnCorrectlyWidth1()
        {
            var result = "A Test".ApplyBorder('#', 1, 0);
            var expected = "########" + Environment.NewLine + "#A Test#" + Environment.NewLine + "########" + Environment.NewLine;
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void BorderDrawnCorrectlyWidth1Padding1()
        {
            var result = "A Test".ApplyBorder('#', 1, 1);
            var expected = 
                  "##########" + Environment.NewLine 
                + "#        #" + Environment.NewLine 
                + "# A Test #" + Environment.NewLine 
                + "#        #" + Environment.NewLine 
                + "##########" + Environment.NewLine;
            Assert.AreEqual(expected, result);
        }



    }
}
