namespace RTWLib_CLI.cmd;

using System;
using System.Collections.Generic;
using System.Reflection;
using RTWLibPlus.helpers;
using System.IO;
using RTWLibPlus.parsers;
using RTWLib_CLI.cmd.screens;
using RTWLib_CLI.draw;
using System.Data;

public static class CMDProcess
{
    private static readonly Templates TemplatesManager = new();
    public static Dictionary<int, string> configs = [];

    public static ModuleRegister modules = new();

    private static string ReadCMD(string cmd, Type type = null)
    {
        int invokeInd = 0;

        string[] cmdSplit = cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (type == null)
        {
            type = Type.GetType("RTWLib_CLI.cmd.modules." + cmdSplit[0], false, true);//   Type.GetType(cmdSplit[0], true, true);
            invokeInd = 1;
        }
        if (type == null)
        {
            return KW.error + ": Type not found";
        }

        object invokableObject = modules.GetModule(type.Name);

        if (cmdSplit.Length < 2)
        {
            return KW.error + ": No command specified";
        }

        foreach (MethodInfo t in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
        {
            if (t.Name.ToLower(System.Globalization.CultureInfo.CurrentCulture) != cmdSplit[invokeInd].ToLower(System.Globalization.CultureInfo.CurrentCulture))
            {
                continue;
            }

            string[] args = cmdSplit.GetItemsFrom(invokeInd + 1);
            ParameterInfo[] par = t.GetParameters();

            if (par.Length > args.Length)
            {
                return string.Format("{0}: Incorrect args, expected: {1}", KW.error, par.ToString(','));
            }

            object[] newArg = new object[par.Length];

            for (int i = 0; i < par.Length; i++)
            {
                type = par[i].ParameterType;

                if (type == typeof(int))
                {
                    newArg[i] = Convert.ToInt32(args[i]);
                }
                else if (type == typeof(string[]))
                {
                    newArg[i] = args.GetItemsFrom(i);
                }
                else
                {
                    newArg[i] = args[i];
                }
            }


            return (string)t.Invoke(invokableObject, newArg);
        }
        return KW.error + ": Command not found, are the arguments correct?";
    }

    public static string CMDScreener(string cmd, Type type = null)
    {
        if (cmd == KW.back)
        { return KW.back; }
        if (cmd == string.Empty)
        { return "no command"; }
        if (cmd == KW.templates)
        { return TemplatesManager.View_Templates(); }
        if (cmd.Split(" ")[0] == "run")
        { return TemplatesManager.Action(cmd); }

        return ReadCMD(cmd, type);

    }

    public static string LoadConfigs()
    {
        if (!Directory.Exists("randomiser_config"))
        {
            return "Config folder does not exist.\nERROR config required. Exiting Program";
        }

        string[] files = Directory.GetFiles("randomiser_config");

        DepthParse dp = new();

        for (int i = 0; i < files.Length; i++)
        {
            string file = files[i];
            string name = Path.GetFileName(file);
            string parse = name;
            configs.Add(i, file);
        }
        return "Configs Loaded";
    }
}
