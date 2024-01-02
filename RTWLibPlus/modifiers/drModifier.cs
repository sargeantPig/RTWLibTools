namespace RTWLibPlus.Modifiers;

using System;
using System.Collections.Generic;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;

public class DRModifier
{

    public DRModifier()
    { }
    /// <summary>
    /// Find regions in DR that aren't used in the descrStrat
    /// </summary>
    /// <param name="currentSettlements">settlements used in the descr_strat</param>
    /// <returns></returns>
    public static List<string> GetMissingRegionNames(List<IBaseObj> currentSettlements, DR dr)
    {
        List<string> regions = dr.Regions;
        List<string> missingRegions = new();

        foreach (string region in regions)
        {
            bool found = false;
            foreach (IBaseObj settlement in currentSettlements)
            {
                string regionName = settlement.Find("region");
                if (regionName == region)
                {
                    found = true;
                }

                if (found)
                {
                    break;
                }

            }
            if (!found)
            {
                missingRegions.Add(region);
            }

        }

        return missingRegions;
    }
}
