namespace RTWLibPlus.Modifiers;

using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;

public class StratModifier
{
    public StratModifier()
    { }

    /// <summary>
    /// Create a settlement given the region name, also requires a dummy settlement to copy the structure
    /// </summary>
    /// <returns>IBaseObj containing settlement structure</returns>
    public static IBaseObj CreateSettlement(IBaseObj dummySettlement, string regionName)
    {
        DSObj dummy = (DSObj)dummySettlement.Copy();
        dummy.FindAndModify("region", regionName);

        return dummy;
    }


}

