using RTWLibPlus.helpers;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.parsers
{
    public static class DepthParse
    {
        public static List<baseObj> Parse(string[] lines)
        {
            int depth = 0;
            int item = 0;

            List<baseObj> list = new List<baseObj>();
            foreach (string line in lines)
            {
                string lineTrim = line.Trim();
                item = list.Count - 1;
                switch (lineTrim) { case "{": depth++; continue; case "}": depth--; continue; }

                string tag = lineTrim.GetFirstWord(' ');
                string value = lineTrim.RemoveFirstWord(' ');
                StoreDataInObject(depth, item, list, tag, value);
            }
            return list;
        }

        private static void StoreDataInObject(int depth, int item, List<baseObj> list, string tag, string value)
        {
            if (depth == 0)
            {
                if (tag != null)
                {
                    list.Add(new baseObj(tag, value, depth));
                }
            }
            else AddWithDepth(list, new baseObj(tag, value, depth), item, depth, 0);
        }

        private static void AddWithDepth(List<baseObj> objs, baseObj obj, int item, int depth, int currentDepth) 
        {
            item = objs.Count-1;
            if (depth != currentDepth)
                AddWithDepth(objs[item].Items, obj, item, depth, ++currentDepth);
            else objs.Add(obj);
        }
    }
}
