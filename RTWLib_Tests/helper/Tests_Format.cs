using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLib_Tests.helper
{
    [TestClass]
    public class Tests_Format
    {

        [TestMethod]
        public void ReturnsCorrectWhiteSpace()
        {
            var result = Format.GetWhiteSpace("type", 20, ' ');
            var expected = "                ";
            Assert.AreEqual(expected, result);
        }
    }
}
