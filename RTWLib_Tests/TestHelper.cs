namespace RTWLib_Tests;

using RTWLibPlus.data;

public static class TestHelper
{
    public static readonly TWConfig Config = TWConfig.LoadConfig(@"resources/remaster.json");
    public static string[] EDB => ["resources", "export_descr_buildings.txt"];
    public static string[] EDU => ["resources", "export_descr_unit.txt"];
    public static string[] SMF => ["resources", "descr_sm_factions.txt"];
    public static string[] DS => ["resources", "descr_strat.txt"];
    public static string[] DMB => ["resources", "descr_model_battle.txt"];
    public static string[] DR => ["resources", "descr_regions.txt"];
}
