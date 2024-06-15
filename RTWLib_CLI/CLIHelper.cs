namespace RTWLib_CLI;

using System;
using System.Reflection;

public static class CLIHelper
{
    public static void ScreenChange(string title)
    {
        //Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();
    }

    public static string ScreenChangeRTN(string title) => title + "\n";


    public static string GetMethodList(Type type)
    {
        string list = string.Empty;
        MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
        int i = -1;
        foreach (MethodInfo method in methods)
        {
            i++;
            if (i == 0)
            {
                continue;
            }

            string methodName = method.ToString().Split(' ')[1];

            list += string.Format("{0}: {1}{2}", i.ToString(), methodName, "\n");
        }
        return list;
    }
}
