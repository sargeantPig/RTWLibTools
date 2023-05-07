using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLib_Tests.helper
{
    [TestClass]
    public class Tests_Array
    {
        [TestMethod]
        public void TotalLengthOfTwoAddedArrays()
        {
            var a = new string[] { "a" , "b", "c" };
            var b = new string[] { "b" , "a", "c" };
            var result = a.Add<string>(b);
            var expected = new string[] { "a" , "b", "c", "b", "a",  "c" };

            CollectionAssert.AreEqual(expected, result);
            Assert.AreEqual(expected.Length, result.Length);
        }

        [TestMethod]
        public void AddValueToArray()
        {
            var a = new string[] { "a", "b", "c" };
            var b = "b";
            var result = a.Add<string>(b);
            var expected = new string[] { "a", "b", "c", "b"};

            CollectionAssert.AreEqual(expected, result);
            Assert.AreEqual(expected.Length, result.Length);
        }
        [TestMethod]
        public void TrimAllInArray()
        {
            var a = new string[] { " a ", " b ", "   c " };
            var result = a.TrimAll();
            var expected = new string[] { "a", "b", "c"};

            CollectionAssert.AreEqual(expected, result);
            Assert.AreEqual(expected.Length, result.Length);
        }



    }
}
