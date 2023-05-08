using RTWLibPlus.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace RTWLibPlus.helpers
{
    public static class exArray
    {

        public static List<IbaseObj> DeepCopy(this List<IbaseObj> list)
        {
            List<IbaseObj> objs = new List<IbaseObj>();

            foreach (var i in list)
            {
                objs.Add(i.Copy());
            }
            return objs;
        }

        public static T[] GetItemsFrom<T>(this T[] values, int startIndex)
        {
            int newLength = values.Length - startIndex;

            if (newLength < 0)
                return new T[0];

            T[] array = new T[newLength];
            int b = 0;
            for(int i = startIndex; i < values.Length; i++)
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
                //str += String.Format("{0}{1}", item, spacer[0]);
            }
            str = str.Trim().TrimEnd(spacer);
            return str;
        }

        public static string ToString(this List<string> array, params char[] spacer)
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

        public static void Shuffle<T>(this IList<T> list, Random rnd)
        {
            for (var i = 0; i < list.Count; i++)
                list.Swap(i, rnd.Next(i, list.Count));
        }

        private static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        public static T GetRandom<T>(this T[] array, out int index, Random rnd)
        {
            index = rnd.Next(array.Count());  
            return array[index];
        }
        public static T GetRandom<T>(this List<T> array, out int index, Random rnd)
        {
            index = rnd.Next(array.Count());
            return array[index];
        }
    }
}
