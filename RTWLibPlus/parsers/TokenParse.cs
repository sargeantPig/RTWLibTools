using RTWLibPlus.helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security;

namespace RTWLibPlus.parsers
{
    public static class TokenParse
    {

        public static Dictionary<string, string[]> ReadAndPrepare(List<Dictionary<string, string[]>> items, string path, char delim, string newIdent)
        {
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
            string[] lines = ReadFile(path);
            foreach (string line in lines)
            {
                KeyValuePair<string, string[]> kv = Prepare(line, delim);
                dict = AddDicToListIfIdent(items, newIdent, dict, kv);
                AddNewKvOrModify(dict, kv);
            }

            items.Add(new Dictionary<string, string[]>(dict));

            return dict;
        }

        private static void AddNewKvOrModify(Dictionary<string, string[]> dict, KeyValuePair<string, string[]> kv)
        {
            if (!dict.ContainsKey(kv.Key))
                dict.Add(kv.Key, kv.Value);
            else dict[kv.Key] = dict[kv.Key].Add(kv.Value);
        }

        private static Dictionary<string, string[]> AddDicToListIfIdent(List<Dictionary<string, string[]>> items, string newIdent, Dictionary<string, string[]> dict, KeyValuePair<string, string[]> kv)
        {
            if (kv.Key == newIdent && dict.ContainsKey(kv.Key))
            {
                items.Add(new Dictionary<string, string[]>(dict));
                dict = new Dictionary<string, string[]>();
            }

            return dict;
        }

        public static string[] ReadFile(string path)
        {
            StreamReader streamReader = new StreamReader(path);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            return GetLines(text);
        }

        public static string ReadFileAsString(string path)
        {
            StreamReader streamReader = new StreamReader(path);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            return text;
        }

        private static string[] GetLines(string text)
        {
            return text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        public static KeyValuePair<string, string[]> Prepare(string line, char delim)
        {
            string firstWord = line.GetFirstWord(' ');
            string value = line.RemoveFirstWord(' ');
            string[] split = value.Split(new char[] { delim, '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var dic = new KeyValuePair<string, string[]>(firstWord, split.ToArray().TrimAll());
            return dic;
        }

    }
}
