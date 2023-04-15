using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Bson;
using RTWLibPlus.helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLib_Tests.helper
{
    [TestClass]
    public class Tests_String
    {
        [TestMethod]
        public void FirstWordRetrieved()
        {
            string testStr = "sheep wool field farm";
            string result = testStr.GetFirstWord(' ');
            string expected = "sheep";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FirstWordRemoved()
        {
            string testStr = "sheep wool field farm";
            string result = testStr.RemoveFirstWord(' ');
            string expected = "wool field farm";
            Assert.AreEqual(expected, result);
        }

    }
}
