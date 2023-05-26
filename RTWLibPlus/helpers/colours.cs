using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using static RTWLibPlus.dataWrappers.TGA;

namespace RTWLibPlus.helpers
{
    public static class Colours
    {
        public static string ColourToString(int r, int g, int b)
        {
            return string.Format("{0}, {1}, {2}", r, g, b);
        }

        public static PIXEL ColourToPixel(this string colourInts)
        {
            if (colourInts.Contains("[") || colourInts.Contains("]"))
                RemoveBracers(ref colourInts);

            string[] split = colourInts.Split(',');

            PIXEL p = new PIXEL();
            p.r = Convert.ToByte(split[0]);
            p.g = Convert.ToByte(split[1]);
            p.b = Convert.ToByte(split[2]);
            return p;
        }

        private static void RemoveBracers(ref string str)
        {
            str = str.Trim(']');
            str = str.Trim('[');
        }

        public static PIXEL Blend(this PIXEL color, PIXEL backColor, double amount)
        {
            PIXEL blended = new PIXEL();
            blended.r = (byte)((color.r * amount) + backColor.r * (1 - amount));
            blended.g = (byte)((color.g * amount) + backColor.g * (1 - amount));
            blended.b = (byte)((color.b * amount) + backColor.b * (1 - amount));

            return blended;
        }

        public static bool EqualTo(this PIXEL a, PIXEL b)
        {
            if (a.r == b.r && a.g == b.g && a.b == b.b)
                return true;
            else return false;

        }
    }
}
