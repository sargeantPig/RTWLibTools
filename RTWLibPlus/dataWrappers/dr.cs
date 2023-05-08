using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.dataWrappers
{
    public class DR : BaseWrapper
    {
        public Dictionary<string, string> RegionsByColour = new Dictionary<string, string>();

        public DR(List<IbaseObj> data)
        {
            this.data = data;
            GetRegionsByColour();
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

        private void GetRegionsByColour()
        {
            int pos = 0;
            for(int i = 0; i < data.Count + 1; i++)
            {
                if (pos == 8)
                {
                    RegionsByColour.Add(((baseObj)data[i-4]).Value, ((baseObj)data[i-pos]).Value);
                    pos = 0;
                }
                pos++;
            }
        }
    }
}
