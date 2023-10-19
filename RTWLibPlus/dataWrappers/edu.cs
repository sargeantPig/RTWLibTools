using RTWLibPlus.data;
using RTWLibPlus.helpers;
using RTWLibPlus.interfaces;
using RTWLibPlus.parsers.objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RTWLibPlus.dataWrappers
{
    public class EDU : BaseWrapper, IWrapper
    {
        private readonly string name = "edu";

        public string GetName()
        {
            return name;
        }
        public EDU(string outputPath, string loadPath)
        {
            OutputPath = outputPath;
            LoadPath = loadPath;
        }

        public EDU(List<IbaseObj> data, TWConfig config)
        {
            this.data = data;
            SetEndOfUnits();
            LoadPath = config.GetPath(Operation.Load, "edu");
            OutputPath = config.GetPath(Operation.Save, "edu");
        }

        public void Parse()
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

            RFH.Write(OutputPath, output + Format.UniversalNewLine());
            return output + Format.UniversalNewLine();
        }

        public void PrepareEDU()
        {
            DeleteChunks("type", "rebalance_statblock");
        }

        public void Clear()
        {
            data.Clear();
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

        public void RemoveAttributesAll(params string[] attriToRemove)
        {
            var attri = GetItemsByIdent("attributes");

            foreach(EDUObj a in attri)
            {
                string[] values = a.Value.Split(',').TrimAll();
                string[] newVals = new string[0];
                foreach(var val in values)
                {
                    if(!attriToRemove.Contains(val))
                    {
                        newVals = newVals.Add(val);
                    }
                }

                a.Value = newVals.ToString(',', ' ');
            }
        }
    }
}
