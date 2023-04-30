using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.dataWrappers
{
    public class DR : BaseWrapper
    {
        public DR(List<IbaseObj> data)
        {
            this.data = data;
        }

        public string Output()
        {
            string output = string.Empty;
            foreach (DRObj obj in data)
            {
                output += obj.Output();
            }
            return output;
        }
    }
}
