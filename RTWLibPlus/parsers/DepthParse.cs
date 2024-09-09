namespace RTWLibPlus.parsers;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class DepthParse
{
    private readonly List<IBaseObj> list = [];
    public delegate IBaseObj ObjectCreator(string value, string tag, int depth);

    public List<IBaseObj> Parse(string[] lines, ObjectCreator creator, char splitter = ' ')
    {
        this.list.Clear();
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
                if (this.list.Count > 0)
                {
                    this.SetNewLinesAfter(depth, whiteSpaceSeparator);
                }

                whiteSpaceSeparator = 0;
            }
            if (line.StartsWith(";"))
            { continue; }
            string lineTrim = line.Trim();
            item = this.list.Count - 1;

            switch (lineTrim.Trim(','))
            {
                case "{":
                    depth++;
                    continue;
                case "}":
                    depth--;
                    continue;
                default:
                    break;
            }

            string tag = lineTrim.GetFirstWord(splitter);
            string value = lineTrim.RemoveFirstWord(splitter);
            this.StoreDataInObject(creator, depth, tag, value);
        }

        return this.list;
    }
    public string[] ReadFile(string path, bool removeEmptyLines = true)
    {
        StreamReader streamReader = new(path, Encoding.UTF8);
        string text = streamReader.ReadToEnd();
        streamReader.Close();

        if (removeEmptyLines)
        {
            return GetLinesRemoveEmpty(text);
        }
        else
        {
            return GetLines(text);
        }
    }
    public static string ReadFileAsString(string path)
    {
        StreamReader streamReader = new(path, Encoding.UTF8);
        string text = streamReader.ReadToEnd();
        streamReader.Close();
        return text;
    }

    private void StoreDataInObject(ObjectCreator creator, int depth, string tag, string value)
    {
        if (depth == 0 && tag != null)
        {
            this.list.Add(creator(value, tag, depth));
        }
        else if (depth > 0)
        {
            AddWithDepth(creator, this.list, depth, 0, tag, value);
        }
    }
    private static void AddWithDepth(ObjectCreator creator, List<IBaseObj> objs, int depth, int currentDepth, string tag, string value)
    {
        int item = objs.Count - 1;
        if (depth != currentDepth)
        {
            AddWithDepth(creator, objs[item].GetItems(), depth, ++currentDepth, tag, value);
        }
        else
        {
            objs.Add(creator(value, tag, depth));
        }
    }
    private void SetNewLinesAfter(int depth, int value)
    {
        int item = this.list.Count - 1;
        if (depth == 0)
        {
            ((BaseObj)this.list[item]).NewLinesAfter = value;
        }
        else if (depth > 0)
        {
            SetNLWithDepth(this.list, depth, value, 0);
        }
    }
    private static void SetNLWithDepth(List<IBaseObj> objs, int depth, int value, int currentDepth)
    {
        int item = objs.Count - 1;
        List<IBaseObj> itesm = objs[item].GetItems();
        if (itesm.Count == 0)
        {
            ((BaseObj)objs[item]).NewLinesAfter = value;
        }
        else if (depth != currentDepth)
        {
            SetNLWithDepth(objs[item].GetItems(), depth, value, ++currentDepth);
        }
        else
        {
            ((BaseObj)objs[item]).NewLinesAfter = value;
        }
    }
    private static string[] GetLinesRemoveEmpty(string text) => text.Split(Format.UniversalNewLine(), StringSplitOptions.RemoveEmptyEntries);
    private static string[] GetLines(string text) => text.Split(Format.UniversalNewLine());
}
