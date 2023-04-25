using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace RTWLibPlus.dataWrappers
{
    public class DS
    {
        public List<IbaseObj> data = new List<IbaseObj>();

        public DS(List<IbaseObj> data)
        {
            this.data = data;
            SetLastOfGroup();
        }

        public string Output()
        {
            string output = string.Empty;
            foreach (DSObj obj in data)
            {
                output += obj.Output();
            }
            return output;
        }

        private void SetLastOfGroup()
        {
            string previous = string.Empty;
            foreach (DSObj obj in data)
            {
                string ident = obj.Tag.Split(DSObj.whiteSpace)[0];

                if (ident != previous)
                    obj.lastOfGroup = true;

                previous = ident;
            }
        }


    }
}
