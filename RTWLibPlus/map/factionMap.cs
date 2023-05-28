using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.randomiser;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Text;
using static RTWLibPlus.dataWrappers.TGA;

namespace RTWLibPlus.map
{
    public class FactionMap
    {
        public void PaintRegionMap(TGA regions, TGA baseMap, DS ds, DR dr, SMF smf, RemasterRome config, string mapDir, string[] factionList)
        {
            Dictionary<string, TGA> factionMaps = initialiseMaps(baseMap, mapDir, factionList);
            TGA complete = baseMap.Copy("", "fullMap.tga");
            Dictionary<string, List<IbaseObj>> settlementsByFaction = ds.GetSettlementsByFaction(config);
            //loop pixels
            for(int i = 0; i < regions.pixels.Length; i++)
            {
                var rp = regions.pixels[i];
                //check for sea or city or port
                if (rp.r == 41 && rp.g == 140 && rp.b == 233)
                    continue;
                if (rp.r == 41 && rp.g == 140 && rp.b == 235)
                    continue;
                if (rp.r == 41 && rp.g == 140 && rp.b == 236)
                    continue;
                if (rp.r == 41 && rp.g == 140 && rp.b == 237)
                    continue;
                if (rp.r == 0 && rp.g == 0 && rp.b == 0)
                    continue;
                if (rp.r == 255 && rp.g == 255 && rp.b == 255)
                    continue;
                //get region by pixel colour
                string region = dr.GetRegionByColour(rp.r, rp.g, rp.b);
                //get which faction has the region
                string faction = ds.GetFactionByRegion(region);
                //get faction colours
                var pri = smf.GetKeyValueAtLocation(smf.data, 0, faction, "colours", "primary").Value;
                var sec = smf.GetKeyValueAtLocation(smf.data, 0, faction, "colours", "secondary").Value;
                PIXEL primaryCol = pri.ColourToPixel();
                PIXEL secondaryCol = sec.ColourToPixel();
                //check for border
                int bc = BorderCheck(i, regions, regions.pixels[i]);
                if (bc >= 1 && bc < 4)
                {
                    //paint border
                    factionMaps[faction].pixels[i] = secondaryCol.Blend(baseMap.pixels[i], 0.6);
                    complete.pixels[i] = secondaryCol.Blend(baseMap.pixels[i], 0.6);
                }
                else
                {
                    //paint pixel on correct faction map
                    factionMaps[faction].pixels[i] = primaryCol.Blend(baseMap.pixels[i], 0.6);
                    complete.pixels[i] = primaryCol.Blend(baseMap.pixels[i], 0.6);
                }
            }
            //output
            foreach(var tga in  factionMaps)
            {
                tga.Value.Output();
            }
            complete.Output();
        }

        private Dictionary<string, TGA> initialiseMaps(TGA baseMap, string mapDir, string[] factionList)
        {
            Dictionary<string, TGA> factionMaps = new Dictionary<string, TGA>();

            foreach (var f in factionList)
            {
                factionMaps.Add(f, baseMap.Copy("", RFH.CurrDirPath(mapDir,
                    string.Format("map_{0}.tga", f))));
            }

            return factionMaps;
        }

        private int BorderCheck(int index, TGA rm, PIXEL mc)
        {
            int bcount = 0;
            var coord = rm.ConvertIndexToCoordinates(index);
            var indLeft = rm.ConvertCoordinatesToIndex(new Vector2(coord.X - 1, coord.Y));
            var indRight = rm.ConvertCoordinatesToIndex(new Vector2(coord.X + 1, coord.Y));
            var indUp = rm.ConvertCoordinatesToIndex(new Vector2(coord.X, coord.Y + 1));
            var indDown = rm.ConvertCoordinatesToIndex(new Vector2(coord.X, coord.Y - 1));
            
            if (!rm.pixels[indRight].EqualTo(mc))
                bcount++;
         
            if (!rm.pixels[indLeft].EqualTo(mc))
                bcount++;
            
            if (!rm.pixels[indUp].EqualTo(mc))
                bcount++;
            
            if (!rm.pixels[indDown].EqualTo(mc))
                bcount++;

            return bcount;

        }

    }
}
