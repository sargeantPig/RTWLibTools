﻿using System;
using System.Collections.Generic;
using System.Text;
using RTWLibPlus.helpers;
namespace RTWLibPlus.edu
{
    public class Unit
    {
        public Dictionary<string, string[]> data { get; set; }

        public Unit(Dictionary<string, string[]> data)
        {
            this.data = data;
        }

        public static Unit[] UnitArray(List<Dictionary<string, string[]>> datas) 
        {
            Unit[] units = new Unit[datas.Count];

            for(int i = 0; i < datas.Count; i++)
            {
                units[i] = new Unit(datas[i]);
            }
            return units;
        }

        public string Output()
        {
            string s = string.Empty;
            foreach(var kv in data)
            {
                s = FormatUnitData(s, kv);
            }
            return s;
        }

        private string FormatUnitData(string s, KeyValuePair<string, string[]> kv)
        {
            if (kv.Key == "ethnicity")
            {
                s = FormatEthnicityTag(s, kv);
            }
            else s += FormatKeyValue(kv);
            return s;
        }

        private string FormatEthnicityTag(string s, KeyValuePair<string, string[]> kv)
        {
            for (int i = 1; i < kv.Value.Length; i += 2)
            {
                s += FormatKeyValueEthnicity(kv.Key, kv.Value[i - 1], kv.Value[i]);
            }
            return s;
        }

        public string FormatKeyValueEthnicity(string key, string a, string b)
        {
            return string.Format("{0}{1}{2}, {3}{4}", key, Format.GetWhiteSpace(key, 20, ' '), a, b, Environment.NewLine);
        }

        public string FormatKeyValue(KeyValuePair<string, string[]> kv)
        {
            return string.Format("{0}{1}{2}{3}", kv.Key, Format.GetWhiteSpace(kv.Key, 20, ' '), kv.Value.ToString(',', ' '), Environment.NewLine);
        }

    }
}
