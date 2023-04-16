using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLib_Tests.dummy;
using RTWLibPlus.edu;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RTWLib_Tests.edu
{
    [TestClass]
    public class Tests_edu
    {
        [TestMethod]
        public void UnitOutputFormatted()
        {
            var datas = new List<Dictionary<string, string[]>>();
            TokenParse.ReadAndPrepare(datas, Path.Combine("resources", "unitExample.txt"), ',', "type");
            var units = Unit.UnitArray(datas);
            var orig = TokenParse.ReadFile(Path.Combine("resources", "unitExample.txt"));
            string origStr = orig.ToString('\r', '\n');
            string result = string.Empty;
            for (int i = 0; i < units.Count(); i++)
            {
                result += units[i].Output();
            }
            result = result.TrimEnd();
            Console.WriteLine(result.Length + " : " + origStr.Length);
            Assert.AreEqual(origStr, result);
        }
        [TestMethod]
        public void UnitReadCorrectly() {
            var result = new List<Dictionary<string, string[]>>();
            TokenParse.ReadAndPrepare( result,Path.Combine("resources","unitExample.txt"), ',', "type");
            var expected = dummyUnit.GetDummy();
            TestHelper.LoopCollectionAssert(expected, result[0]);
            Assert.AreEqual(2, result.Count);

        }
        [TestMethod]
        public void FileReadLineArray()
        {
            string[] result = TokenParse.ReadFile(Path.Combine("resources", "inputTest.txt"));
            string[] expected = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
