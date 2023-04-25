using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace RTWLibPlus.dataWrappers.edb
{
    public class EDB : BaseWrapper
    {

        public EDB(List<IbaseObj> data)
        {
            this.data = data;
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
            return output;
        }

    }
}
