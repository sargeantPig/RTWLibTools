using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTWLibPlus.dataWrappers
{
    public class EDU : BaseWrapper, IWrapper
    {

        public EDU() {
            LoadPath = RemasterRome.GetPath(false, "edu");
            OutputPath = RemasterRome.GetPath(true, "edu");
        }
        public EDU(List<IbaseObj> data)
        {
            this.data = data;
            SetEndOfUnits();
            LoadPath = RemasterRome.GetPath(false, "edu");
            OutputPath = RemasterRome.GetPath(true, "edu");
        }

        public void Parse(string path = "")
        {
            this.data = RFH.ParseFile(Creator.EDUcreator, ' ', false, LoadPath);
            SetEndOfUnits();
        }

        public string Output()
        {
            string output = string.Empty;
            foreach (EDUObj obj in data)
            {
                output += obj.Output();
            }
            RFH.Write(OutputPath, output + Environment.NewLine);
            return output + Environment.NewLine;
        }

        private void SetEndOfUnits()
        {
            for(int i = 0; i < data.Count; i++)
            {
                if (i + 1 >= data.Count)
                    return;

                EDUObj obj = (EDUObj)data[i];
                EDUObj nextObj = (EDUObj)data[i + 1];
                if (nextObj.Tag == "type" || nextObj.Tag == "rebalance_statblock"  || nextObj.Tag == "is_female" ) {

                    obj.endOfUnit = true;
                }
            }
        }
    }
}
