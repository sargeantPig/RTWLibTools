using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Text;
using static RTWLibPlus.interfaces.IbaseObj;

namespace RTWLibPlus.parsers
{
    public static class DepthParse
    {
     public delegate IbaseObj ObjectCreator(string value, string tag, int depth);

    public static List<IbaseObj> Parse(string[] lines, ObjectCreator creator)
    {
        int depth = 0;
        int item = 0;

        List<IbaseObj> list = new List<IbaseObj>();
        foreach (string line in lines)
        {
            if (line.StartsWith(";")) { continue; }
            string lineTrim = line.TrimStart();
            item = list.Count - 1;

            switch (lineTrim) { case "{": depth++; continue; case "}": depth--; continue; }

            string tag = lineTrim.GetFirstWord(' ');
            string value = lineTrim.RemoveFirstWord(' ');
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
    }
}
