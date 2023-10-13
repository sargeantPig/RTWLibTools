using RTWLib_CLI;
using RTWLib_CLI.cmd;
using RTWLib_CLI.cmd.modules;
using RTWLib_CLI.draw;
using RTWLib_CLI.input;
using RTWLibPlus.data;
using RTWLibPlus.helpers;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RTWLIB_CLI
{
    class Program
    {
        private static string title = "Welcome to the RTWLib CLI\n     By Sargeant Pig\n---\ntype 'help' for commands and usage";
        private static string configTitle = string.Format("Please select a config from below using the number{0}{1}",
                "\n", CMDProcess.configs.DictToString());

        static void Main(string[] args)
        {
            Console.WriteLine(CMDProcess.LoadTemplates());
            Console.WriteLine(CMDProcess.LoadConfigs());

            if(CMDProcess.configs.Count == 0)
                Environment.Exit(1);

            CLIHelper.ScreenChange(title.ApplyBorder('#', 2, 1));
            int input = Input.GetIntInput(configTitle, x => x >= 0 && x < CMDProcess.configs.Count);
            TWConfig config = TWConfig.LoadConfig(CMDProcess.configs[input]);
            RandCMD rand = new RandCMD(config);
            Console.WriteLine("Config Loaded".ApplyBorder('#', 1, 1));
            CMDProcess.modules.RegisterModule(rand);
            //Rand.InitialSetup();
            while (true)
            {
                string ret = CMDProcess.ReadCMD(Console.ReadLine());

                if (ret != KW.back) { Console.WriteLine(ret.ApplyBorder('=', 1, 1)); continue; }
                CLIHelper.ScreenChange(title);
            }  
        }

    }
}
