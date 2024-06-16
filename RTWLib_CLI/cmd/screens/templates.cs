namespace RTWLib_CLI.cmd.screens;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RTWLib_CLI.draw;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers;

public class Templates
{
    private static readonly CompositeFormat TemplatesTitle = CompositeFormat.Parse("Please select a template from below\nby typing the following 'run template_name.txt': {0}{1}");

    private readonly Dictionary<string, string[]> templates = [];

    private readonly string title;
    public Templates()
    {
        this.LoadTemplates();
        this.title = string.Format(null, TemplatesTitle, "\n", this.templates.Keys.ToArray().ArrayToString(false, true, true, 1, false, false));
    }

    public string Action(string cmd)
    {
        string[] cmdSplit = cmd.Split(" ");
        if (cmdSplit.Length == 1)
        {
            return "run command invalid";
        }

        if (this.templates.ContainsKey(cmdSplit[1]))
        {
            return this.ProcessTemplate(cmdSplit[1]);
        }

        else
        {
            return string.Format("Template not found: {0}", cmdSplit[1]);
        }
    }

    public string View_Templates() => CLIHelper.ScreenChangeRTN(this.title.ApplyBorder('=', 1, 1));

    private string ProcessTemplate(string template)
    {
        string[] cmds = this.templates[template];
        Console.WriteLine("Running: " + template);
        //Progress p = new(1f / cmds.Length, "Running: " + template);
        foreach (string cmd in cmds)
        {
            Console.WriteLine("Doing: " + cmd);
            //p.Message("Doing: " + cmd);
            CMDProcess.CMDScreener(cmd);
            //p.Update("Complete");
        }
        return "template finished processing";
    }

    private string LoadTemplates()
    {
        if (!Directory.Exists("randomiser_templates"))
        {
            return "Template folder does not exist. Skipping template loading";
        }

        string[] files = Directory.GetFiles("randomiser_templates");

        DepthParse dp = new();

        foreach (string file in files)
        {
            string name = Path.GetFileName(file);
            string[] parse = dp.ReadFile(file);
            this.templates.Add(name, parse);
        }
        return "Templates Loaded";
    }

}

