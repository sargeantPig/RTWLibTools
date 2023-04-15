using RTWLibPlus.parsers.obj;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.edb
{
    public class EDB
    {
        public List<baseObj> data = new List<baseObj>();

        public EDB(List<baseObj> data) {
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
