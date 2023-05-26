using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using static RTWLibPlus.dataWrappers.TGA;

namespace RTWLibPlus.dataWrappers
{
    public class DR : BaseWrapper, IWrapper
    {
        public Dictionary<string, string> RegionsByColour = new Dictionary<string, string>();

        public DR() {
            OutputPath = RemasterRome.GetPath(true, "dr");
            LoadPath = RemasterRome.GetPath(false, "dr");
        }

        public DR(List<IbaseObj> data)
        {
            this.data = data;
            GetRegionsByColour();
            OutputPath = RemasterRome.GetPath(true, "dr");
            LoadPath = RemasterRome.GetPath(false, "dr");
        }

        public void Clear()
        {
            data.Clear();
            RegionsByColour.Clear();
        }

        public void Parse(string path = "") {

            this.data = RFH.ParseFile(Creator.DRcreator, '\t', false, LoadPath);
            GetRegionsByColour();
        
        } 

        public string GetRegionByColour(int r, int g, int b) {
            string key = string.Format("{0} {1} {2}", r, g, b);
            string region = RegionsByColour[key];
            return region;
        }

        public string Output()
        {
            string output = string.Empty;
            foreach (DRObj obj in data)
            {
                output += obj.Output();
            }
            RFH.Write(OutputPath, output);

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
