using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using RTWLibPlus.helpers;
using System.Linq;
using RTWLIB_CLI;
using System.IO;
using RTWLibPlus.parsers;
using RTWLib_CLI.draw;
using System.Security;

namespace RTWLib_CLI.cmd
{
    public static class CMDProcess
    {
        public static Dictionary<string, string[]> templates = new Dictionary<string, string[]>();
        public static Dictionary<int, string> configs = new Dictionary<int, string>();

        public static ModuleRegister modules = new ModuleRegister();

        public static string ReadCMD(string cmd, Type type = null)
        {
            if (cmd == KW.back) { return KW.back; }
            if (cmd == KW.help) { return Help.help(); }
            if (cmd == string.Empty) { return "no command"; }
            if (templates.ContainsKey(cmd))
                return ProcessTemplate(cmd);
            int invokeInd = 0;

            string[] cmdSplit = cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (type == null)
            {
                type = Type.GetType("RTWLib_CLI.cmd.modules." + cmdSplit[0], false, true);//   Type.GetType(cmdSplit[0], true, true);
                invokeInd = 1;
            }
            if (type == null)
                return KW.error + ": Type not found";

            var invokableObject = modules.GetModule(type.Name);


            foreach (MethodInfo t in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (t.Name.ToLower() != cmdSplit[invokeInd].ToLower()) continue;

                string[] args = cmdSplit.GetItemsFrom(invokeInd + 1);
                var par = t.GetParameters();

                if (par.Length > args.Length)
                {
                    return string.Format("{0}: Incorrect args, expected: {1}", KW.error, par.ToString(','));
                }

                object[] newArg = new object[par.Length];

                for (int i = 0; i < par.Length; i++)
                {
                    type = par[i].ParameterType;

                    if (type == typeof(Int32))
                        newArg[i] = Convert.ToInt32(args[i]);
                    else if (type == typeof(string[]))
                    {
                        newArg[i] = args.GetItemsFrom(i);
                    }
                    else newArg[i] = args[i];

                }


                return (string)t.Invoke(invokableObject, newArg);
            }
            return KW.error + ": Command not found, are the arguments correct?";
        }

        public static string ProcessTemplate(string template)
        {
            var cmds = templates[template];
            Progress p = new Progress(1f / (cmds.Count()), "Running: " + template);
            foreach (var cmd in cmds)
            {
                p.Message("Doing: " + cmd);
                ReadCMD(cmd);
                p.Update("Complete");
            }
            return "template finished processing";
        }

        public static string LoadTemplates()
        {
            if (!Directory.Exists("randomiser_templates"))
                return "Template folder does not exist. Skipping template loading";

            var files = Directory.GetFiles("randomiser_templates");

            DepthParse dp = new DepthParse();

            foreach (var file in files)
            {
                string name = Path.GetFileName(file);
                var parse = dp.ReadFile(file);
                templates.Add(name, parse);
            }
            return "Templates Loaded";
        }

        public static string LoadConfigs()
        {
            if (!Directory.Exists("randomiser_config"))
                return "Config folder does not exist.\nERROR config required. Exiting Program";

            var files = Directory.GetFiles("randomiser_config");

            DepthParse dp = new DepthParse();

            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                string name = Path.GetFileName(file);
                var parse = file;
                configs.Add(i, parse);
            }
            return "Configs Loaded";
        }
    }
}
