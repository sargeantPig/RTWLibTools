using RTWLib_CLI.draw;
using RTWLibPlus.helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RTWLib_CLI.cmd
{
    public static class Help
    {
        private static bool open = false;
        private static string title;
        public static string help()
        {
            string methods = GetMethodList();
            title = string.Format("RTWLib CLI\nHelp Screen\n---\nTry the following commands in\nformat [name] [arg1] [arg2] etc\n---\n{0}", methods).ApplyBorder('#', 2);

            if (open) { 
                return "Already on Help screen"; }
            open = true;

            CLIHelper.ScreenChange(title);
            while (true)
            {
                string ret = CMDProcess.ReadCMD(Console.ReadLine(), typeof(Help));
                if(ret == KW.back) { open = false; return KW.back; }
                Console.WriteLine(ret.ApplyBorder('~', 1, 1));  
            }
        }

        public static string Test(params string[] args) {
            string argsStr = args.ToString(',');
            return "response: " + argsStr;
        }

        private static string GetMethodList()
        {
            string list = string.Empty;
            var type = typeof(Help);
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            int i = -1;
            foreach ( var method in methods )
            {
                i++;
                if (i == 0)
                    continue;

                string methodName = method.ToString().Split(' ')[1];

                list += string.Format("{0}: {1}{2}", i.ToString(),  methodName, "\n");
            }
            return list;
        }
    }
}
