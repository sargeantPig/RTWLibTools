using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RTWLib_Tests
{
    public static class TestHelper
    {
        public static string[] GetValues(this KeyValuePair<string, string[]> dict)
        {
            return dict.Value;
        }

        public static void LoopCollectionAssert(Dictionary<string, string[]> expected, Dictionary<string, string[]> result)
        {
            var lstKeysExpected = expected.Keys.ToList();
            var lstKeysResult = result.Keys.ToList();
            var lstValuesExpected = expected.Values.ToList();
            var lstValuesResult = result.Values.ToList();

            for (int i = 0; i < expected.Count; i++){
                CollectionAssert.AreEqual(lstValuesExpected[i], lstValuesResult[i]);
            }

            CollectionAssert.AreEqual(lstKeysExpected.ToArray(), lstKeysResult.ToArray());
        }
    }
}
