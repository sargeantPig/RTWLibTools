namespace RTWLibPlus.helpers;

public static class ExString
{
    public static string CrossPlatPath(string path) //converts path to use /
    {
        char[] chars = path.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            char c = chars[i];
            if (c == '\\')
            {
                chars[i] = '/';
            }
        }
        return chars.ToString();
    }
    public static string GetFirstWord(this string str, char delim) => str.Split(delim)[0];
    public static string RemoveFirstWord(this string str, char delim)
    {
        int endsAt = str.IndexOf(delim);

        if (endsAt == -1)
        {
            return str;
        }

        string newStr = str[endsAt..].Trim(delim);

        return newStr;
    }

    public static string Trim(this string str, int start, int end)
    {
        string newString = string.Empty;

        for (int i = 0; i < str.Length; i++)
        {
            if (i >= start && i <= end)
            {
                newString += str[i];
            }
        }

        return newString;
    }

    public static string CRL(this string str, int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            str += "\r\n";
        }
        return str;
    }
}
