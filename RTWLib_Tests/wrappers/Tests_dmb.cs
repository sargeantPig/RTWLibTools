namespace RTWLib_Tests.wrappers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.dataWrappers;

[TestClass]
public class Tests_dmb
{
    [TestMethod]
    public void FallbackForAllTextures()
    {
        DMB descr_model_battle = Instance.InstanceDMB(TestHelper.Config, TestHelper.DMB);
        int textureCountOrig = descr_model_battle.GetItemsByIdent("texture").Count;
        int typeCountOrig = descr_model_battle.GetItemsByIdent("type").Count;
        descr_model_battle.AddFallBacksForAllTypes();
        int textureCountNew = descr_model_battle.GetItemsByIdent("texture").Count;

        int diff = textureCountNew - textureCountOrig;

        Assert.AreEqual(typeCountOrig - descr_model_battle.NoDefaultNeeded, diff, 1);

    }

}
