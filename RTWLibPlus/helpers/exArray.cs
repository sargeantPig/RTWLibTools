namespace RTWLibPlus.helpers;
using RTWLibPlus.data.unit;
using RTWLibPlus.interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

public static class ExArray
{
    public static Dictionary<TK, T> InitDictFromList<TK, T>(this TK[] arr, T def)
    {
        Dictionary<TK, T> keyValuePairs = new();
        foreach (TK o in arr)
        {
            keyValuePairs.Add(o, def);
        }
        return keyValuePairs;
    }

    public static void Init<T>(this List<T[]> list, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            list.Add(Array.Empty<T>());
        }
    }

    public static void Init(this List<Unit> list, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            list.Add(new Unit());
        }
    }

    public static List<IBaseObj> DeepCopy(this List<IBaseObj> list)
    {
        List<IBaseObj> objs = new();

        foreach (IBaseObj i in list)
        {
            objs.Add(i.Copy());
        }
        return objs;
    }

    public static T[] GetItemsFrom<T>(this T[] values, int startIndex)
    {
        int newLength = values.Length - startIndex;

        if (newLength < 0)
        {
            return Array.Empty<T>();
        }

        T[] array = new T[newLength];
        int b = 0;
        for (int i = startIndex; i < values.Length; i++)
        {
            array[b] = values[i];
            b++;
        }
        return array;
    }

    public static T[] GetItemsFromFirstOf<T>(this T[] values, int occurHash)
    {
        T[] array = Array.Empty<T>();
        bool copy = false;
        for (int i = 0; i < values.Length; i++)
        {
            int valHash = values[i].GetHashCode();
            if (valHash == occurHash)
            {
                copy = true;
            }

            if (copy)
            {
                array = array.Add(values[i]);
            }
        }
        return array;
    }

    public static T[] Add<T>(this T[] values, T value)
    {
        T[] array = new T[values.Length + 1];

        if (values.Length > 0)
        {
            values.CopyTo(array, 0);
        }

        array[^1] = value;
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
            for (int i = 0; i < index; i++)
            {
                array[i] = values[i];
            }
        }

        for (int i = index + 1; i < values.Length; i++)
        {
            if (i <= values.Length - 1)
            {
                array[i - 1] = values[i];
            }
        }
        return array;
    }

    public static int Find(this string[] array, string value)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Trim() == value.Trim())
            {
                return i;
            }
        }
        return -1;
    }

    public static int Find(this int[] array, int value)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == value)
            {
                return i;
            }
        }
        return -1;
    }

    public static string[] FindAndRemove(this string[] array, string value)
    {
        int index = array.Find(value);

        if (index == -1)
        {
            return array;
        }

        string[] newArr = array.Remove(index);
        return newArr;
    }

    public static string[] TrimAll(this string[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = array[i].Trim();
        }
        return array;
    }

    public static int GetLongestLength(this string[] array)
    {
        int length = 0;
        foreach (string s in array)
        {
            if (length < s.Length)
            {
                length = s.Length;
            }
        }
        return length;
    }

    public static string ToString(this string[] array, params char[] spacer)
    {
        string str = string.Empty;
        string strSpacer = spacer.ConstructStringFromChars();
        foreach (string item in array)
        {

            str += string.Format("{0}{1}", item, strSpacer);
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
            str += string.Format("{0}{1}", item, strSpacer);
        }
        str = str.Trim().TrimEnd(spacer);
        return str;
    }

    public static string ToString(this ParameterInfo[] array, params char[] spacer)
    {
        string str = string.Empty;
        string strSpacer = spacer.ConstructStringFromChars();
        foreach (ParameterInfo item in array)
        {
            str += string.Format("{0} {1}{2} ", item.ParameterType, item.Name, strSpacer);
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
        for (int i = 0; i < list.Count; i++)
        {
            list.Swap(i, rnd.Next(i, list.Count));
        }
    }

    public static void ShuffleMany<T>(this IList<T>[] list, Random rnd)
    {
        for (int i = 0; i < list[0].Count; i++)
        {
            int s = rnd.Next(i, list[0].Count);
            for (int l = 0; l < list.Length; l++)
            {
                list[l].Swap(i, s);
            }
        }
    }


    private static void Swap<T>(this IList<T> list, int i, int j) => (list[j], list[i]) = (list[i], list[j]);

    public static T GetRandom<T>(this T[] array, out int index, Random rnd)
    {
        index = rnd.Next(array.Length);
        return array[index];
    }
    public static T GetRandom<T>(this List<T> array, out int index, Random rnd)
    {
        index = rnd.Next(array.Count);
        return array[index];
    }

    public static string DictToString<T, TX>(this Dictionary<T, TX> dict)
    {
        string newString = string.Empty;
        foreach (KeyValuePair<T, TX> kv in dict)
        {
            newString += string.Format("{0}: {1}{2}", kv.Key.ToString(), kv.Value.ToString(), "\n");
        }
        return newString;
    }
}
