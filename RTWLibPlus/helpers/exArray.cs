using RTWLibPlus.data.unit;
using RTWLibPlus.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;

namespace RTWLibPlus.helpers
{
    public static class exArray
    {
        public static Dictionary<K, T> InitDictFromList<K, T>(this K[] arr, T def)
        {
            Dictionary<K, T> keyValuePairs = new Dictionary<K, T>();
            foreach(var o in arr)
            {
                keyValuePairs.Add(o, def);
            }
            return keyValuePairs;
        }

        public static void init<T>(this List<T[]> list, int amount)
        {
            for(int i = 0; i < amount; i++)
                list.Add(new T[0]);
        }

        public static void init(this List<Unit> list, int amount)
        {
            for (int i = 0; i < amount; i++)
                list.Add(new Unit());
        }

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

        public static T[] GetItemsFromFirstOf<T>(this T[] values, int occurHash)
        {
            T[] array = new T[0];
            bool copy = false;
            for (int i = 0; i < values.Length; i++)
            {
                var valHash = values[i].GetHashCode();
                if (valHash == occurHash)
                    copy = true;
                if (copy)
                {
                    array = array.Add(values[i]);
                }
            }
            return array;
        }

        public static T[] Add<T>(this T[] values, T value)
        {
            T[] array = new T[values.Length+1];

            if(values.Length > 0)
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

        public static T[] Remove<T>(this T[] values, int index)
        {
            T[] array = new T[values.Length - 1];

            if (index > 0)
            { 
                for(int i = 0; i < index; i++)
                {
                    array[i] = values[i];
                }
            }

            for(int i = index+1; i < values.Length; i++)
            {
                if(i <= values.Length-1)
                    array[i-1] = values[i];
            }
            return array;
        }

        public static int Find(this string[] array, string value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Trim() == value.Trim()) 
                    return i;
            }
            return -1;
        }

        public static int Find(this int[] array, int value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == value) return i;
            }
            return -1;
        }

        public static string[] FindAndRemove(this string[] array, string value)
        {
            int index = array.Find(value);

            if(index == -1)
                return array;

            var newArr = array.Remove(index);
            return newArr;
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

        public static string ToString(this ParameterInfo[] array, params char[] spacer)
        {
            string str = string.Empty;
            string strSpacer = spacer.ConstructStringFromChars();
            foreach (var item in array)
            {
                str += String.Format("{0} {1}{2} ", item.ParameterType, item.Name, strSpacer);
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

        public static void ShuffleMany<T>(this IList<T>[] list, Random rnd)
        {
            for (var i = 0; i < list[0].Count; i++)
            {
                int s = rnd.Next(i, list[0].Count);
                for (var l = 0; l < list.Count(); l++)
                {
                    list[l].Swap(i, s);
                }
            }
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

        public static string DictToString<T, X>(this Dictionary<T, X> dict)
        {
            string newString = string.Empty;
            foreach( var kv in dict)
            {
                newString += string.Format("{0}: {1}{2}", kv.Key.ToString(), kv.Value.ToString(), "\n");
            }
            return newString;
        }
    }
}
