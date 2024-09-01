namespace RTWLib_CLI.cmd;

using System;
using System.Collections.Generic;

public class ModuleRegister
{
    private readonly List<object> modules = [];
    public ModuleRegister() { }

    public void RegisterModule(object module)
    {
        this.modules.Add(module);
        Console.WriteLine("Registered Module: " + module.GetType().Name);
    }

    public object GetModule(string name) => this.modules.Find(x => x.GetType().Name == name);
}
