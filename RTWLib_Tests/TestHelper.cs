namespace RTWLib_Tests;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using RTWLibPlus.interfaces;
using System.Reflection;

public static class TestHelper
{
    public static string[] GetValues(this KeyValuePair<string, string[]> dict) => dict.Value;

    public static void LoopCollectionAssert(Dictionary<string, string[]> expected, Dictionary<string, string[]> result)
    {
        List<string> lstKeysExpected = expected.Keys.ToList();
        List<string> lstKeysResult = result.Keys.ToList();
        List<string[]> lstValuesExpected = expected.Values.ToList();
        List<string[]> lstValuesResult = result.Values.ToList();

        for (int i = 0; i < expected.Count; i++)
        {
            CollectionAssert.AreEqual(lstValuesExpected[i], lstValuesResult[i]);
        }

        CollectionAssert.AreEqual(lstKeysExpected.ToArray(), lstKeysResult.ToArray());
    }

    public static void LoopListAssert(List<IBaseObj> expected, List<IBaseObj> result)
    {
        Type t = result[0].GetType();

        for (int i = 0; i < result.Count; i++)
        {
            foreach (PropertyInfo property in t.GetProperties())
            {
                var expVal = property.GetValue(expected[i]);
                var resVal = property.GetValue(result[i]);
                Assert.AreEqual(property.GetValue(expected[i]), property.GetValue(result[i]));
            }
        }
    }
}
