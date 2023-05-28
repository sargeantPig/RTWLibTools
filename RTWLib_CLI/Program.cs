﻿using RTWLib_CLI;
using RTWLib_CLI.cmd;
using RTWLib_CLI.draw;
using RTWLibPlus.data;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RTWLIB_CLI
{
    class Program
    {
        private static string title = "Welcome to the RTWLib CLI\n     By Sargeant Pig\n---\ntype 'help' for commands and usage";

        static void Main(string[] args)
        {
            CMDProcess.LoadTemplates();
            CMDProcess.LoadConfigs();
            
            RemasterRome config = RemasterRome.LoadConfig(@"randomiser_config\remaster.json");
            RandCMD rand = new RandCMD(config);
            CMDProcess.modules.RegisterModule(rand);

            CLIHelper.ScreenChange(title.ApplyBorder('#', 2, 1));
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
