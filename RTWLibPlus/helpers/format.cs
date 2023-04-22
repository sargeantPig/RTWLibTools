using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RTWLibPlus.helpers
{
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

    }
}
