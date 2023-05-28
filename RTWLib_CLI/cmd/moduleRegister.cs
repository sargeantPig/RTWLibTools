using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLib_CLI.cmd
{
    public class ModuleRegister
    {
        List<Object> modules = new List<Object>();
        public ModuleRegister() { }

        public void RegisterModule(Object module)
        {
            modules.Add(module);
        }

        public Object GetModule(string name)
        {
            return modules.Find(x => x.GetType().Name == name);
        }
    }
}
