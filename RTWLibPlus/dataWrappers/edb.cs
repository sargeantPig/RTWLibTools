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
        public EDB() {
            LoadPath = RemasterRome.GetPath(false, "edb");
            OutputPath = RemasterRome.GetPath(true, "edb");
        }
        public EDB(List<IbaseObj> data)
        {
            this.data = data;
            Sanitise(data);
            LoadPath = RemasterRome.GetPath(false, "edb");
            OutputPath = RemasterRome.GetPath(true, "edb");
        }
        public void Parse(string path = "")
        {
            this.data = RFH.ParseFile(Creator.EDBcreator, ' ', false, LoadPath);
            Sanitise(data);
        }
        private void Sanitise(List<IbaseObj> toSanitise)
        {
            foreach (baseObj obj in toSanitise)
            {
                if(obj.Tag == "building" || obj.Value == "building" )
                {
                    Swap(obj);
                }
            }
        }

        private void Swap(baseObj obj)
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
