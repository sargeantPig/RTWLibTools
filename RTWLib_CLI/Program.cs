namespace RTWLIB_CLI;
using RTWLib_CLI;
using RTWLib_CLI.cmd;
using RTWLib_CLI.cmd.modules;
using RTWLib_CLI.draw;
using RTWLib_CLI.input;
using RTWLibPlus.data;
using RTWLibPlus.helpers;
using System;
using System.IO;


internal class Program
{
    private static readonly string Title = "Welcome to the RTWLib CLI\n     By Sargeant Pig\n---\ntype 'help' for commands and usage";
    private static readonly string ConfigTitle = string.Format("Please select a config from below using the number{0}{1}",
            "\n", CMDProcess.configs.DictToString());

    private static void Main(string[] args)
    {
        string wdir = AppDomain.CurrentDomain.BaseDirectory;
        Directory.SetCurrentDirectory(wdir);

        Console.WriteLine(CMDProcess.LoadConfigs());

        if (CMDProcess.configs.Count == 0)
        {
            Environment.Exit(1);
        }

        CLIHelper.ScreenChange(Title.ApplyBorder('#', 2, 1));
        int input = Input.GetIntInput(ConfigTitle, x => x >= 0 && x < CMDProcess.configs.Count);
        TWConfig config = TWConfig.LoadConfig(CMDProcess.configs[input]);
        RandCMD rand = new(config);
        Help help = new();
        Console.WriteLine("Config Loaded".ApplyBorder('#', 1, 1));
        CMDProcess.modules.RegisterModule(rand);
        CMDProcess.modules.RegisterModule(help);

        Console.WriteLine(CMDProcess.CMDScreener("templates"));
        //Rand.InitialSetup();
        while (true)
        {
            string ret = CMDProcess.CMDScreener(Console.ReadLine());

            if (ret != KW.back)
            { Console.WriteLine(ret.ApplyBorder('=', 1, 1)); continue; }
            CLIHelper.ScreenChange(Title);
        }
    }

}
