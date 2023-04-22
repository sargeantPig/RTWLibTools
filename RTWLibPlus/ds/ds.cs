using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.ds
{
    public class DS
    {
        public List<IbaseObj> data = new List<IbaseObj>();

        public DS(List<IbaseObj> data)
        {
            this.data = data;
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
    }
}
