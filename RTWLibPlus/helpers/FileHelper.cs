using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static RTWLibPlus.parsers.DepthParse;
using System.Runtime.InteropServices.WindowsRuntime;

namespace RTWLibPlus.helpers
{
    public static class RFH
    {
        public static void Write(string path, string content)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.Write(content);
            sw.Flush();
            sw.Close();
        }

        public static string CurrDirPath(params string[] path) {
            string finpath = Directory.GetCurrentDirectory();
            foreach (string s in path)
            {
                finpath = Path.Combine(finpath, s);
            }
            return finpath;
        }

        public static string ConstructPath(params string[] path)
        {
            string finpath = string.Empty;
            foreach (string s in path)
            {
                finpath = Path.Combine(finpath, s);
            }
            return finpath;
        }

        public static List<IbaseObj> ParseFile(ObjectCreator creator, char splitter = ' ',  bool removeEmptyLines = false, params string[] path)
        {
            var fileLines = DepthParse.ReadFile(RFH.CurrDirPath(path), removeEmptyLines);
            var parsed = DepthParse.Parse(fileLines, creator, splitter);

            return parsed;
        }

        public static string GetPartOfPath(string path, string from)
        {
            if (path == null)
                return "";

            string[] split = path.Split('\\');

            var arr = split.GetItemsFromFirstOf(from.GetHashCode());

            var str = RFH.ConstructPath(arr);
            return string.Format("..\\{0}", str);


        }
    }
}
