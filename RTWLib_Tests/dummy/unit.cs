namespace RTWLib_Tests.dummy;

using System.Collections.Generic;

public static class dummyUnit
{
    public static Dictionary<string, string[]> GetDummy() => new()
        {
            {"type", new string[]{ "roman hastati"} },
            {"dictionary", new string[]{"roman_hastati" } },
            {"category", new string[]{ "infantry" } },
            {"class", new string[]{"light"} },
            {"voice_type", new string[]{ "Light_1" } },
            {"voice_indexes", new string[]{ "0 1 2" } },
            {"soldier", new string[]{ "roman_hastati", "40", "0", "1" } },
            {"attributes", new string[]{ "power_charge", "hide_anywhere", "sea_faring", "mercenary_unit" } },
            {"formation", new string[]{ "1", "2", "2", "3", "4", "square" } },
            {"stat_health", new string[]{ "1", "0" } },
            {"stat_pri", new string[]{ "11", "2", "pilum", "35", "2", "thrown", "blade", "piercing", "spear", "25", "1" } },
            {"stat_pri_attr", new string[]{ "prec", "thrown ap" } },
            {"stat_sec", new string[]{ "7", "2", "no", "0", "0", "melee", "simple", "piercing", "sword", "25", "1" } },
            {"stat_sec_attr", new string[]{ "no" } },
            {"stat_pri_armour", new string[]{ "5", "4", "5", "metal" } },
            {"stat_sec_armour", new string[]{ "0", "1", "flesh" } },
            {"stat_heat", new string[]{ "3" } },
            {"stat_ground", new string[]{ "2", "0", "0", "0" } },
            {"stat_mental", new string[]{ "6", "normal", "trained" } },
            {"stat_charge_dist", new string[]{ "30" } },
            {"stat_fire_delay", new string[]{ "0" } },
            {"stat_food", new string[]{ "60", "300" } },
            {"stat_cost", new string[]{ "1", "440", "170", "50", "70", "440" } },
            {"ownership", new string[]{ "thrace", "romans_scipii", "slave" } },
            {"ethnicity", new string[]{ "romans_brutii", "Bruttium", "romans_julii", "Etruria" } },
        };
}
