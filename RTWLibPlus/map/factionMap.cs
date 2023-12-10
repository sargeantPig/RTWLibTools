namespace RTWLibPlus.map;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using System.Numerics;
using System.Collections.Generic;
using RTWLibPlus.dataWrappers.TGA;
using System.Threading.Tasks;

public class FactionMap
{

    public void PaintRegionMap(TGA regions, TGA baseMap, DS ds, DR dr, SMF smf, string mapDir)
    {
        Dictionary<string, TGA> factionMaps = this.InitialiseMaps(baseMap, mapDir, smf.GetFactions().ToArray());
        TGA complete = baseMap.Copy("", "fullMap.tga");
        Dictionary<string, List<IBaseObj>> settlementsByFaction = ds.GetSettlementsByFaction(smf);
        //loop Pixels
        Parallel.For(0, regions.Pixels.Length, i =>
        {
            PIXEL rp = regions.Pixels[i];
            //check for sea or city or port
            if (rp.R == 41 && rp.G == 140 && rp.B == 233)
            {
                return;
            }

            if (rp.R == 41 && rp.G == 140 && rp.B == 235)
            {
                return;
            }

            if (rp.R == 41 && rp.G == 140 && rp.B == 236)
            {
                return;
            }

            if (rp.R == 41 && rp.G == 140 && rp.B == 237)
            {
                return;
            }

            if (rp.R == 0 && rp.G == 0 && rp.B == 0)
            {
                return;
            }

            if (rp.R == 255 && rp.G == 255 && rp.B == 255)
            {
                return;
            }
            //get region by pixel colour
            string region = dr.GetRegionByColour(rp.R, rp.G, rp.B);
            //get which faction has the region
            string faction = ds.GetFactionByRegion(region);
            //get faction colours
            string pri = smf.GetKeyValueAtLocation(smf.Data, 0, faction, "colours", "primary").Value;
            string sec = smf.GetKeyValueAtLocation(smf.Data, 0, faction, "colours", "secondary").Value;
            PIXEL primaryCol = pri.ColourToPixel();
            PIXEL secondaryCol = sec.ColourToPixel();
            //check for border
            int bc = this.BorderCheck(i, regions, regions.Pixels[i]);
            if (bc is >= 1 and < 4)
            {
                //paint border
                factionMaps[faction].Pixels[i] = secondaryCol.Blend(baseMap.Pixels[i], 0.6);
                complete.Pixels[i] = secondaryCol.Blend(baseMap.Pixels[i], 0.6);
            }
            else
            {
                //paint pixel on correct faction map
                factionMaps[faction].Pixels[i] = primaryCol.Blend(baseMap.Pixels[i], 0.6);
                complete.Pixels[i] = primaryCol.Blend(baseMap.Pixels[i], 0.6);
            }
        });
        //output
        foreach (KeyValuePair<string, TGA> tga in factionMaps)
        {
            tga.Value.Output();
        }
        complete.Output();
    }

    private Dictionary<string, TGA> InitialiseMaps(TGA baseMap, string mapDir, string[] factionList)
    {
        Dictionary<string, TGA> factionMaps = new();

        foreach (string f in factionList)
        {
            factionMaps.Add(f, baseMap.Copy("", RFH.CurrDirPath(mapDir,
                string.Format("map_{0}.tga", f))));
        }

        return factionMaps;
    }

    private int BorderCheck(int index, TGA rm, PIXEL mc)
    {
        int bcount = 0;
        Vector2 coord = rm.ConvertIndexToCoordinates(index);
        int indLeft = rm.ConvertCoordinatesToIndex(new Vector2(coord.X - 1, coord.Y));
        int indRight = rm.ConvertCoordinatesToIndex(new Vector2(coord.X + 1, coord.Y));
        int indUp = rm.ConvertCoordinatesToIndex(new Vector2(coord.X, coord.Y + 1));
        int indDown = rm.ConvertCoordinatesToIndex(new Vector2(coord.X, coord.Y - 1));

        if (!rm.Pixels[indRight].EqualTo(mc))
        {
            bcount++;
        }

        if (!rm.Pixels[indLeft].EqualTo(mc))
        {
            bcount++;
        }

        if (!rm.Pixels[indUp].EqualTo(mc))
        {
            bcount++;
        }

        if (!rm.Pixels[indDown].EqualTo(mc))
        {
            bcount++;
        }

        return bcount;

    }

}
