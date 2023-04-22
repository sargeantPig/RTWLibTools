﻿using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.edb
{
    public class EDB
    {
        public List<IbaseObj> data = new List<IbaseObj>();

        public EDB(List<IbaseObj> data) {
            this.data = data;
        
        
        }
       
        public string Output()
        {
            string output = string.Empty;
            foreach (EDBObj obj in data)
            {
                output += obj.Output();
            }
            return output;
        }

    }
}
