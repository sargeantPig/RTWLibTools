using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace RTWLibPlus.dataWrappers
{
    public class EDB : BaseWrapper, IWrapper
    {
        private readonly string name = "edb";

        public string GetName()
        {
            return name;
        }

        public EDB(string outputPath, string loadPath)
        {
            OutputPath = outputPath;
            LoadPath = loadPath;
        }

        public EDB(List<IbaseObj> data)
        {
            this.data = data;
            Sanitise(data);
            LoadPath = RemasterRome.GetPath(Operation.Load, "edb");
            OutputPath = RemasterRome.GetPath(Operation.Save, "edb");
        }
        public void Parse()
        {
            this.data = RFH.ParseFile(Creator.EDBcreator, ' ', false, LoadPath);
            Sanitise(data);
        }

        public void Clear()
        {
            data.Clear();
        }

        private void Sanitise(List<IbaseObj> toSanitise)
        {
            foreach (BaseObj obj in toSanitise)
            {
                if(obj.Tag == "building" || obj.Value == "building" )
                {
                    Swap(obj);
                }
            }
        }

        private void Swap(BaseObj obj)
        {
            string temp = obj.Tag;
            obj.Tag = obj.Value;
            obj.Value = temp;
        }

        public string Output()
        {
            Sanitise(data);
            string output = string.Empty;
            foreach (EDBObj obj in data)
            {
                output += obj.Output();
            }
            RFH.Write(OutputPath, output);
            return output;
        }
    }
}
