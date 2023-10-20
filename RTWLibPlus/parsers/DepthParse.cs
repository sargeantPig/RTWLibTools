using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using static RTWLibPlus.interfaces.IbaseObj;

namespace RTWLibPlus.parsers
{
    public class DepthParse
    {
        List<IbaseObj> list = new List<IbaseObj>();

        public delegate IbaseObj ObjectCreator(string value, string tag, int depth);
        public List<IbaseObj> Parse(string[] lines, ObjectCreator creator, char splitter = ' ')
        {
            list.Clear();
            int depth = 0;
            int item = 0;
            int whiteSpaceSeparator = 0;
            
            foreach (string line in lines)
            {
                string lineTrimEnd = line.TrimEnd();

                if ((lineTrimEnd == string.Empty || line == Format.UniversalNewLine()) && !line.StartsWith(";"))
                {
                    whiteSpaceSeparator++;
                    continue;
                }
                else
                {
                    if (list.Count > 0)
                        SetNewLinesAfter(depth, whiteSpaceSeparator);
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
                StoreDataInObject(creator, depth, tag, value);
            }
        
            return list;
        }
        public string[] ReadFile(string path, bool removeEmptyLines = true)
        {
            StreamReader streamReader = new StreamReader(path, Encoding.UTF8);
            string text = streamReader.ReadToEnd();
            streamReader.Close();

            if (removeEmptyLines)
                return GetLinesRemoveEmpty(text);
            else return GetLines(text);
        }
        public string ReadFileAsString(string path)
        {
            StreamReader streamReader = new StreamReader(path,  Encoding.UTF8);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            return text;
        }

        private void StoreDataInObject(ObjectCreator creator, int depth, string tag, string value)
        {
            if (depth == 0 && tag != null)
                list.Add(creator(value, tag, depth));
            else if (depth > 0)
                AddWithDepth(creator, list, depth, 0, tag, value);
        }
        private void AddWithDepth(ObjectCreator creator, List<IbaseObj> objs, int depth, int currentDepth, string tag, string value)
        {
            int item = objs.Count - 1;
            if (depth != currentDepth)
                AddWithDepth(creator, objs[item].GetItems(), depth, ++currentDepth, tag, value);
            else objs.Add(creator(value, tag, depth));
        }
        private void SetNewLinesAfter(int depth, int value)
        {
            int item = list.Count - 1;
            if (depth == 0)
                ((BaseObj)list[item]).NewLinesAfter = value;
            else if (depth > 0)
                SetNLWithDepth(list, depth, value, 0);
        }
        private void SetNLWithDepth(List<IbaseObj> objs, int depth, int value, int currentDepth)
        {
            int item = objs.Count - 1;
            var itesm = objs[item].GetItems();
            if (itesm.Count == 0)
                ((BaseObj)objs[item]).NewLinesAfter = value;
            else if (depth != currentDepth)
                SetNLWithDepth(objs[item].GetItems(), depth, value, ++currentDepth);
            else ((BaseObj)objs[item]).NewLinesAfter = value;

        }
        private string[] GetLinesRemoveEmpty(string text)
        {
            return text.Split(Format.UniversalNewLine(), StringSplitOptions.RemoveEmptyEntries);
        }
        private string[] GetLines(string text)
        {
            return text.Split(Format.UniversalNewLine());
        }
    }
}
