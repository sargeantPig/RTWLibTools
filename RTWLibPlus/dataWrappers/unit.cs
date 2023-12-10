namespace RTWLibPlus.edu;
using System.Collections.Generic;
using RTWLibPlus.helpers;

public class Unit
{
    public Dictionary<string, string[]> Data { get; set; }

    public Unit(Dictionary<string, string[]> data) => this.Data = data;

    public static Unit[] UnitArray(List<Dictionary<string, string[]>> datas)
    {
        Unit[] units = new Unit[datas.Count];

        for (int i = 0; i < datas.Count; i++)
        {
            units[i] = new Unit(datas[i]);
        }
        return units;
    }

    public string Output()
    {
        string s = string.Empty;
        foreach (KeyValuePair<string, string[]> kv in this.Data)
        {
            s = this.FormatUnitData(s, kv);
        }
        return s;
    }

    private string FormatUnitData(string s, KeyValuePair<string, string[]> kv)
    {
        if (kv.Key == "ethnicity")
        {
            s = this.FormatEthnicityTag(s, kv);
        }
        else
        {
            s += this.FormatKeyValue(kv);
        }

        return s;
    }

    private string FormatEthnicityTag(string s, KeyValuePair<string, string[]> kv)
    {
        for (int i = 1; i < kv.Value.Length; i += 2)
        {
            s += this.FormatKeyValueEthnicity(kv.Key, kv.Value[i - 1], kv.Value[i]);
        }
        return s;
    }

    public string FormatKeyValueEthnicity(string key, string a, string b) => string.Format("{0}{1}{2}, {3}{4}", key, Format.GetWhiteSpace(key, 20, ' '), a, b, Format.UniversalNewLine());

    public string FormatKeyValue(KeyValuePair<string, string[]> kv) => string.Format("{0}{1}{2}{3}", kv.Key, Format.GetWhiteSpace(kv.Key, 20, ' '), kv.Value.ToString(',', ' '), Format.UniversalNewLine());

}
