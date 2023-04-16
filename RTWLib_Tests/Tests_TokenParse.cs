using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RTWLib_Tests
{
    [TestClass]
    public class Tests_TokenParse
    {
        [TestMethod]
        public void PrepareReturnsDicInFormat()
        {
            string toParse = "soldier             barb_peasant, 60, 0, 0.7";
            KeyValuePair<string, string[]> result = TokenParse.Prepare(toParse, ',');
            var expected = new KeyValuePair<string, string[]>();
            expected = new KeyValuePair<string, string[]>("soldier", new string[] { "barb_peasant", "60", "0", "0.7" });

            CollectionAssert.AreEqual(expected.GetValues(), result.GetValues());
        }
        [TestMethod]
        public void FileReadLineArray()
        {
            string[] result = TokenParse.ReadFile(Path.Combine("resources", "inputTest.txt"));
            string[] expected = new string[] {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

            CollectionAssert.AreEqual(expected, result);
        }
    
    }
}
