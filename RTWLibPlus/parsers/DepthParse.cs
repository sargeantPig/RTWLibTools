using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using static RTWLibPlus.interfaces.IbaseObj;

namespace RTWLibPlus.parsers
{
    public static class DepthParse
    {
        public delegate IbaseObj ObjectCreator(string value, string tag, int depth);

        public static List<IbaseObj> Parse(string[] lines, ObjectCreator creator, char splitter = ' ')
        {
            int depth = 0;
            int item = 0;
            int whiteSpaceSeparator = 0;
            List<IbaseObj> list = new List<IbaseObj>();
            foreach (string line in lines)
            {
                string lineTrimEnd = line.TrimEnd();

                if ((lineTrimEnd == string.Empty || line == Environment.NewLine[0].ToString()) && !line.StartsWith(";"))
                {
                    whiteSpaceSeparator++;
                    continue;
                }
                else
                {
                    if (list.Count > 0)
                        SetNewLinesAfter(list, depth, item, whiteSpaceSeparator);
                    whiteSpaceSeparator = 0;
                }
                if (line.StartsWith(";")) { continue; }
                string lineTrim = line.Trim();
                item = list.Count - 1;

                switch (lineTrim.Trim(','))
                {
                    case "{": depth++; continue;
                    case "}": depth--; continue;
                }



                string tag = lineTrim.GetFirstWord(splitter);
                string value = lineTrim.RemoveFirstWord(splitter);
                StoreDataInObject(creator, depth, item, list, tag, value);
            }
        
            return list;
        }

        private static void StoreDataInObject(ObjectCreator creator, int depth, int item, List<IbaseObj> list, string tag, string value)
        {
            if (depth == 0 && tag != null)
                list.Add(creator(value, tag, depth));
            else if (depth > 0)
                AddWithDepth(creator, list, item, depth, 0, tag, value);
        }

        private static void AddWithDepth(ObjectCreator creator, List<IbaseObj> objs, int item, int depth, int currentDepth, string tag, string value)
        {
            item = objs.Count - 1;
            if (depth != currentDepth)
                AddWithDepth(creator, objs[item].GetItems(), item, depth, ++currentDepth, tag, value);
            else objs.Add(creator(value, tag, depth));
        }

        private static void SetNewLinesAfter(List<IbaseObj> list, int depth, int item, int value)
        {
            item = list.Count - 1;
            if (depth == 0)
                ((baseObj)list[item]).newLinesAfter = value;
            else if (depth > 0)
                SetNLWithDepth(list, depth, item, value, 0);
        }

        private static void SetNLWithDepth(List<IbaseObj> list, int depth, int item, int value, int currentDepth)
        {
            item = list.Count - 1;
            var itesm = list[item].GetItems();
            if (itesm.Count == 0)
                ((baseObj)list[item]).newLinesAfter = value;
            else if (depth != currentDepth)
                SetNLWithDepth(list[item].GetItems(), depth, item, value, ++currentDepth);
            else ((baseObj)list[item]).newLinesAfter = value;

        }
    }
}
