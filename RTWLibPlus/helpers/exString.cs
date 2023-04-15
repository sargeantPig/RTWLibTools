using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.helpers
{
    public static class exString
    {
        public static string GetFirstWord(this string str, char delim)
        {
            return str.Split(delim)[0];
        }
        public static string RemoveFirstWord(this string str, char delim)
        {
            int endsAt = str.IndexOf(delim);
            if(endsAt == -1)
                return str;


            return str.Substring(endsAt).Trim();
        }
    }
}
