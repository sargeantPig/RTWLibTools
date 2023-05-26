using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System.Collections.Generic;
using System.Linq;

namespace RTWLibPlus.dataWrappers
{
    public class SMF : BaseWrapper, IWrapper
    {

        public SMF() {
            LoadPath = RemasterRome.GetPath(false, "smf");
            OutputPath = RemasterRome.GetPath(true, "smf");
        }    
        public SMF(List<IbaseObj> data)
        {
            this.data = data;
            Sanitise(data);
            LoadPath = RemasterRome.GetPath(false, "smf");
            OutputPath = RemasterRome.GetPath(true, "smf");
        }
        public void Parse(string path = "")
        {
            this.data = RFH.ParseFile(Creator.BaseCreator, ':', false, LoadPath);
            Sanitise(data);
        }
        public string Output()
        {
            string output = string.Empty;
            foreach (baseObj obj in data)
            {
                output += obj.Output();
            }
            RFH.Write(OutputPath, output);
            return output;
        }

        public void Clear()
        {
            data.Clear();
        }

        private void Sanitise(List<IbaseObj> toSanitise)
        {
            foreach (baseObj obj in toSanitise)
            {
                obj.Value = obj.Value.Trim();
                obj.Tag = obj.Tag.Trim();
                obj.Ident = obj.Ident.Trim();
                obj.Value = obj.Value.Trim(',');
                obj.Value = obj.Value.Trim(':');
                obj.Value = obj.Value.Trim('"');
                obj.Tag = obj.Tag.Trim(',');
                obj.Tag = obj.Tag.Trim(':');
                obj.Tag = obj.Tag.Trim('"');
                obj.Ident = obj.Ident.Trim(',');
                obj.Ident = obj.Ident.Trim(':');
                obj.Ident = obj.Ident.Trim('"');
                Sanitise(obj.GetItems());
            }
        }
    }
}
