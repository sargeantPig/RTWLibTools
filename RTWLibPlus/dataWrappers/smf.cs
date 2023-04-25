using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System.Collections.Generic;
using System.Linq;

namespace RTWLibPlus.dataWrappers
{
    public class SMF : BaseWrapper
    {
        public SMF(List<IbaseObj> data)
        {
            this.data = data;
            Sanitise(data);
        }

        public string Output()
        {
            string output = string.Empty;
            foreach (baseObj obj in data)
            {
                output += obj.Output();
            }
            return output;
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
