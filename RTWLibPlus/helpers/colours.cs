namespace RTWLibPlus.helpers;
using System;
using RTWLibPlus.dataWrappers.TGA;

public static class Colours
{
    public static string ColourToString(int r, int g, int b) => string.Format("{0}, {1}, {2}", r, g, b);

    public static PIXEL ColourToPixel(this string colourInts)
    {
        if (colourInts.Contains('[') || colourInts.Contains(']'))
        {
            RemoveBracers(ref colourInts);
        }

        string[] split = colourInts.Split(',');

        PIXEL p = new()
        {
            R = Convert.ToByte(split[0]),
            G = Convert.ToByte(split[1]),
            B = Convert.ToByte(split[2])
        };
        return p;
    }

    private static void RemoveBracers(ref string str)
    {
        str = str.Trim(']');
        str = str.Trim('[');
    }

    public static PIXEL Blend(this PIXEL color, PIXEL backColor, double amount)
    {
        PIXEL blended = new()
        {
            R = (byte)((color.R * amount) + (backColor.R * (1 - amount))),
            G = (byte)((color.G * amount) + (backColor.G * (1 - amount))),
            B = (byte)((color.B * amount) + (backColor.B * (1 - amount)))
        };

        return blended;
    }

    public static bool EqualTo(this PIXEL a, PIXEL b)
    {
        if (a.R == b.R && a.G == b.G && a.B == b.B)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
