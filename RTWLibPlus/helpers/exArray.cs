using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace RTWLibPlus.helpers
{
    public static class exArray
    {

        public static T[] GetItemsFrom<T>(this T[] values, int index)
        {
            int newLength = values.Length - index;

            if (newLength < 0)
                return new T[0];

            T[] array = new T[newLength];
            int b = 0;
            for(int i = index; i < values.Length; i++)
            {
                array[b] = values[i];
                b++;
            }
            return array;
        }

        public static T[] Add<T>(this T[] values, T value)
        {
            T[] array = new T[values.Length+1];
            values.CopyTo(array, 0);
            array[array.Length-1] = value;
            return array;
        }

        public static T[] Add<T>(this T[] orig, T[] value)
        {
            T[] array = new T[orig.Length + value.Length];
            
            orig.CopyTo(array, 0);
            value.CopyTo(array, orig.Length);

            return array;
        }

        public static string[] TrimAll(this string[] array)
        {
            for (int i = 0;i < array.Length; i++)
            {
                array[i] = array[i].Trim();
            }
            return array;
        }

        public static int GetLongestLength(this string[] array)
        {
            int length = 0;
            foreach(string s in array)
            {
                if(length < s.Length)
                    length = s.Length;
            }
            return length;
        }

        public static string ToString(this string[] array, params char[] spacer)
        {
            string str = string.Empty;
            string strSpacer = spacer.ConstructStringFromChars();
            foreach (string item in array)
            {
                str += String.Format("{0}{1}", item, strSpacer);
            }
            str = str.Trim().TrimEnd(spacer);
            return str;
        }

        public static string ConstructStringFromChars(this char[] chars)
        {
            string str = string.Empty;
            foreach (char c in chars)
            {
                str += c;
            }
            return str;
        }
    }
}
