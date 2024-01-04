namespace RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers;
using System.Collections.Generic;
using System.IO;
using static RTWLibPlus.parsers.DepthParse;

public static class RFH
{
    public static void Write(string path, string content)
    {
        StreamWriter sw = new(path);
        sw.Write(content);
        sw.Flush();
        sw.Close();
    }

    public static string CurrDirPath(params string[] path)
    {
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

    public static List<IBaseObj> ParseFile(ObjectCreator creator, char splitter = ' ', bool removeEmptyLines = false, params string[] path)
    {
        DepthParse dp = new();
        string[] fileLines = dp.ReadFile(CurrDirPath(path), removeEmptyLines);
        List<IBaseObj> parsed = dp.Parse(fileLines, creator, splitter);

        return parsed;
    }

    public static string GetPartOfPath(string path, string from)
    {
        if (path == null)
        {
            return "";
        }

        string[] split = path.Split('\\', '/');

        string[] arr = split.GetItemsFromFirstOf(from.GetHashCode());

        string str = ConstructPath(arr);
        return string.Format("../{0}", str);


    }
}
