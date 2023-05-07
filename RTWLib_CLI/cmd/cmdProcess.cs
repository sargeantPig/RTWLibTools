using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using RTWLibPlus.helpers;
using System.Linq;
using RTWLIB_CLI;

namespace RTWLib_CLI.cmd
{
    public static class CMDProcess
    {
        public static string ReadCMD(string cmd, Type type = null)
        {
            if(cmd == KW.back) { return KW.back; }
            if(cmd == KW.help) { return Help.help(); }
            if(cmd == string.Empty) { return "no command"; }
            int invokeInd = 0;

            string[] cmdSplit = cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (type == null)
            {
                type = Type.GetType("RTWLib_CLI.cmd." + cmdSplit[0], false, true);//   Type.GetType(cmdSplit[0], true, true);
                invokeInd = 1;
            }
            if (type == null)
                return KW.error + ": Type not found";

            foreach(MethodInfo t in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (t.Name.ToLower() != cmdSplit[invokeInd].ToLower()) continue;

                string[] args = cmdSplit.GetItemsFrom(invokeInd + 1);

                var par = t.GetParameters();
                object[] newArg = new object[par.Length];

                for(int i =0; i < par.Length; i++)
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


                return (string)t.Invoke(new object(), newArg);              
            }
            return KW.error + ": Command not found, are the arguments correct?";
        }
    }
}
