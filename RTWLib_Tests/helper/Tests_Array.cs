namespace RTWLib_Tests.helper;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.helpers;
using System.Collections.Generic;

[TestClass]
public class Tests_Array
{
    [TestMethod]
    public void TotalLengthOfTwoAddedArrays()
    {
        string[] a = new string[] { "a", "b", "c" };
        string[] b = new string[] { "b", "a", "c" };
        string[] result = a.Add(b);
        string[] expected = new string[] { "a", "b", "c", "b", "a", "c" };

        CollectionAssert.AreEqual(expected, result);
        Assert.AreEqual(expected.Length, result.Length);
    }

    [TestMethod]
    public void AddValueToArray()
    {
        string[] a = new string[] { "a", "b", "c" };
        string b = "b";
        string[] result = a.Add(b);
        string[] expected = new string[] { "a", "b", "c", "b" };

        CollectionAssert.AreEqual(expected, result);
        Assert.AreEqual(expected.Length, result.Length);
    }
    [TestMethod]
    public void TrimAllInArray()
    {
        string[] a = new string[] { " a ", " b ", "   c " };
        string[] result = a.TrimAll();
        string[] expected = new string[] { "a", "b", "c" };

        CollectionAssert.AreEqual(expected, result);
        Assert.AreEqual(expected.Length, result.Length);
    }
    [TestMethod]
    public void RemoveFromArrayMiddle()
    {
        string[] arr = new string[] { "a", "b", "c", "d", "e", "f", "g" };
        string[] result = arr.Remove(3);
        string[] expected = new string[] { "a", "b", "c", "e", "f", "g" };
        CollectionAssert.AreEqual(expected, result);
    }
    [TestMethod]
    public void RemoveFromArrayEnd()
    {
        string[] arr = new string[] { "a", "b", "c", "d", "e", "f", "g" };
        string[] result = arr.Remove(6);
        string[] expected = new string[] { "a", "b", "c", "d", "e", "f" };
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void RemoveFromArrayAlmostEnd()
    {
        string[] arr = new string[] { "a", "b", "c", "d", "e", "f", "g" };
        string[] result = arr.Remove(5);
        string[] expected = new string[] { "a", "b", "c", "d", "e", "g" };
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void RemoveFromArrayStart()
    {
        string[] arr = new string[] { "a", "b", "c", "d", "e", "f", "g" };
        string[] result = arr.Remove(0);
        string[] expected = new string[] { "b", "c", "d", "e", "f", "g" };
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void FindAndRemove()
    {
        string[] arr = new string[] { "a", "b", "c", "d", "e", "f", "g" };
        string[] result = arr.FindAndRemove("c");
        string[] expected = new string[] { "a", "b", "d", "e", "f", "g" };
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void FindAndRemoveOddFormat()
    {
        string[] arr = new string[] { "a", "b", " c ", "d", "e", "f", "g" };
        string[] result = arr.FindAndRemove("c");
        string[] expected = new string[] { "a", "b", "d", "e", "f", "g" };
        CollectionAssert.AreEqual(expected, result);
    }
}
