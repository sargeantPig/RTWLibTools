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

            string newStr = str.Substring(endsAt).Trim(delim);

            return newStr;
        }

        public static string Trim(this string str, int start, int end)
        {
            string newString = string.Empty;
            
            for(int i = 0; i < str.Length; i++)
            {
                if(i >= start && i <= end)
                {
                    newString += str[i];
                }
            }

            return newString;
        }
    }
}
