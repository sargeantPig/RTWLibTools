namespace RTWLib_Tests.helper;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.helpers;

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
