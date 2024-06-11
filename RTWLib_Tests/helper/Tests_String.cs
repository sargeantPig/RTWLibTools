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

    // [TestMethod]
    // public void PartOfPath()
    // {
    //     string test2 = Environment.CurrentDirectory;
    //     string test = "drive\\path\\long\\this\\something\\randomiser\\files\\somefile.txt";
    //     string result = RFH.GetPartOfPath(test, "randomiser");
    //     Assert.AreEqual("../randomiser/files/somefile.txt", result);
    // }

}
