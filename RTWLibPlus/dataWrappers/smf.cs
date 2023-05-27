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
        private readonly string name = "smf";

        public string GetName()
        {
            return name;
        }
        public SMF(string outputPath, string loadPath)
        {
            OutputPath = outputPath;
            LoadPath = loadPath;
        }


        public SMF(List<IbaseObj> data)
        {
            this.data = data;
            Sanitise(data);
            LoadPath = RemasterRome.GetPath(Operation.Load, "smf");
            OutputPath = RemasterRome.GetPath(Operation.Save, "smf");
        }
        public void Parse()
        {
            this.data = RFH.ParseFile(Creator.SMFcreator, ':', false, LoadPath);
            Sanitise(data);
        }
        public string Output()
        {
            string output = string.Empty;
            foreach (BaseObj obj in data)
            {
                //output += obj.Output();
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
            foreach (BaseObj obj in toSanitise)
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
