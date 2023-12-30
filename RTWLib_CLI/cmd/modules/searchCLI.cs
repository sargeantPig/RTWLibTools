namespace RTWLib_CLI.cmd.modules;

using RTWLibPlus.data;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System.Collections.Generic;

public class Search
{
    TWConfig config;

    public Search(TWConfig twConfig) => this.config = twConfig;
    /// <summary>
    /// -id file acronym, -m -v (mod or vanilla), -w find what (can be multiple values separate by comma),
    /// </summary>
    /// <param name="args"></param>
    public void Find(params string[] args)
    {
        IWrapper wrapper = null;

        switch (args[0])
        {
            case "edu":
                wrapper = new EDU(this.SetFilePath(args[0], args[1]), this.SetFilePath(args[0], args[1]));
                break;
            case "edb":
                break;
            case "ds":
                break;
            case "smf":
                break;
            case "dr":
                break;
            default:
                break;
        }

        wrapper.Parse();

        List<List<BaseObj>> results = new();

        for (int i = 3; i < args.Length; i++)
        {
            ((BaseWrapper)wrapper).GetItemsByIdent(args[i]);
        }



        foreach (string arg in args)
        {


        }
    }

    private string SetFilePath(string file, string vanOrMod)
    {
        Operation operation = Operation.Load;

        if (vanOrMod == "-m")
        {
            operation = Operation.Save;
        }
        else if (vanOrMod == "-v")
        {
            operation = Operation.Load;
        }

        return this.config.GetPath(operation, file);
    }

}
