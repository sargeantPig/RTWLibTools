namespace RTWLibPlus.helpers;
using System.Runtime.InteropServices;

public static class Format
{
    public static string GetWhiteSpace<T>(string tag, int end, T white)
    {
        int tagL = tag.Length;
        int diff = end - tagL;
        string whiteSpace = GetStringOf(white, diff);
        return whiteSpace;

    }

    public static string GetStringOf<T>(T character, int length)
    {
        string str = string.Empty;
        for (int i = 0; i < length; i++)
        {
            str += character;
        }
        return str;
    }

    public static string UniversalNewLine()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "\r\n";
        }
        else
        {
            return "\r\n"; // environment.newline
        }
    }

}
