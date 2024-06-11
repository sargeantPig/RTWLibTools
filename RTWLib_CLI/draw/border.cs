namespace RTWLib_CLI.draw;

using RTWLibPlus.helpers;
using System;

public static class Border
{
    public static string ApplyBorder(this string str, char borderChar, int borderWidth, int padding = 1)
    {
        string top = string.Empty;
        string bottom = string.Empty;
        string right = string.Empty;
        string pad = Format.GetStringOf(' ', padding);
        string bord = Format.GetStringOf(borderChar, borderWidth);
        string title = string.Empty;
        string[] lineSplit = str.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        int lengthOfStr = lineSplit.GetLongestLength();
        int borderLength = lengthOfStr + (borderWidth * 2) + (padding * 2);
        int wSLength = borderLength - (borderWidth * 2);

        right = bord + Format.UniversalNewLine();

        foreach (string line in lineSplit)
        {
            if (line.Length > lengthOfStr)
            {
                lengthOfStr = line.Length;
            }
        }

        for (int i = 0; i < lineSplit.Length; i++)
        {
            int diff = lengthOfStr - lineSplit[i].Length;
            string rightPad = Format.GetStringOf(' ', padding + diff);
            title += bord + pad + lineSplit[i] + rightPad + right;
        }

        for (int i = 0; i < borderWidth; i++)
        {
            top += Format.GetStringOf(borderChar, borderLength) + Format.UniversalNewLine();
            bottom += Format.GetStringOf(borderChar, borderLength) + Format.UniversalNewLine();
        }

        for (int i = 0; i < padding + lineSplit.Length - lineSplit.Length; i++)
        {
            top += bord + Format.GetStringOf(' ', wSLength) + bord + Format.UniversalNewLine();
            bottom = bord + Format.GetStringOf(' ', wSLength) + bord + Format.UniversalNewLine() + bottom;
        }

        return string.Format("{0}{1}{2}", top, title, bottom);
    }

}
