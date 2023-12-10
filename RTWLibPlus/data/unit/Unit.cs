namespace RTWLibPlus.data.unit;
using RTWLibPlus.data.unit.unit_data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;

public class Unit : IComparable<Unit>
{
    /// <summary>
    /// string internal name of the unit.
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// tag for looking up the screen name
    /// </summary>
    public string Dic { get; set; }
    /// <summary>
    /// infantry, cavalry, siege, handler, ship or non_combatant
    /// </summary>
    public string Category { get; set; }
    /// <summary>
    /// light, heavy, spearman, or missle
    /// </summary>
    public string UClass { get; set; }
    /// <summary>
    /// determines the type of voice used by the unit
    /// </summary>
    public string Voice { get; set; }
    /// <summary>
    /// make-up of each soldier (name, number, collision etc)
    /// </summary>
    public string[] Soldier { get; set; }
    /// <summary>
    /// name of officer model. There may be up to 0-3 officer lines per unit
    /// </summary>
    public string[] Officer { get; set; }
    /// <summary>
    /// type of siege used ( ballista, scorpion, onager, heavy_onager, repeating_ballista)
    /// </summary>
    public string Engine { get; set; }
    /// <summary>
    /// the type of non-ridden on animals used by the  wardogs, pigs
    /// </summary>
    public string Animal { get; set; }
    /// <summary>
    /// type of animal or vehicle ridden on
    /// </summary>
    public string Mount { get; set; }
    /// <summary>
    /// type of ship used if applicable
    /// </summary>
    public string Naval { get; set; }
    /// <summary>
    /// modifiers vs different mounts
    /// </summary>
    public MountEffect MountEffect { get; set; }
    /// <summary>
    /// A list of attributes and abilities the unit may have.Including
    /// sea_faring = can board ships
    /// hide_forest, hide_improved_forest, hide_long_grass, hide_anywhere   = defines where the unit can hide
    /// can_sap = Can dig tunnels under walls
    /// frighten_foot, frighten_mounted = Cause fear to certain nearby unit types
    /// can_run_amok = Unit may go out of control when riders lose control of animals
    /// general_unit = The unit can be used for a named character's bodyguard
    /// cantabrian_circle = The unit has this special ability
    /// no_custom = The unit may not be selected in custom battles
    /// command = The unit carries a legionary eagle, and gives bonuses to nearby units
    /// mercenary_unit = The unit is s mercenary unit available to all factions
    /// </summary>
    public string[] Attributes { get; set; }
    /// <summary>
    /// formation values
    /// </summary>
    public string[] Formation { get; set; }
    /// <summary>
    /// [0] hit postring of man
    /// [1] hitpostring of mount or attached animal (horses and camels do not have separate hitpostrings.
    /// </summary>
    public string[] Health { get; set; }
    /// <summary>
    /// primary weapon stats
    /// </summary>
    public string[] PriWep { get; set; }
    /// <summary>
    /// primary weapon attributes any or all of
    /// ap = armour piercing.Only counts half of target's armour
    /// bp = body piercing.Missile can pass through men and hit those behind
    /// spear = Used for long spears.Gives bonuses fighting cavalry, and penalties against infantry
    /// long_pike = Use very long pikes.Phalanx capable units only
    /// short_pike = Use shorter than normal spears.Phalanx capable units only
    /// prec = Missile weapon is only thrown just before charging stringo combat
    /// thrown = The missile type if thrown rather than fired
    /// launching = attack may throw target men stringo the air
    /// area = attack affects an area, not just one man
    /// </summary>
    public string[] PriAttri { get; set; }
    /// <summary>
    /// secondary weapon stats
    /// </summary>
    public string[] SecWep { get; set; }
    /// <summary>
    /// primary weapon attributes any or all of
    /// ap = armour piercing.Only counts half of target's armour
    /// bp = body piercing.Missile can pass through men and hit those behind
    /// spear = Used for long spears.Gives bonuses fighting cavalry, and penalties against infantry
    /// long_pike = Use very long pikes.Phalanx capable units only
    /// short_pike = Use shorter than normal spears.Phalanx capable units only
    /// prec = Missile weapon is only thrown just before charging stringo combat
    /// thrown = The missile type if thrown rather than fired
    /// launching = attack may throw target men stringo the air
    /// area = attack affects an area, not just one man
    /// </summary>
    public string[] SecAttri { get; set; }
    /// <summary>
    /// details of the units defences
    /// </summary>
    public string[] PriArm { get; set; }
    /// <summary>
    /// Details of animal's or vehicle's defenses (note riden horses do not have a separate defence)
    /// </summary>
    public string[] SecArmr { get; set; }
    /// <summary>
    /// Extra fatigue suffered by the unit in hot climates
    /// </summary>
    private string Heat { get; set; }
    /// <summary>
    /// combat modifiers for different ground types
    /// [0] scrub modifier
    /// [1] sand modifier
    /// [2] forest modifier
    /// [3] snow modifier
    /// </summary>
    public string[] Ground { get; set; }
    /// <summary>
    /// mental stats of the unit
    /// </summary>
    public string[] Mental { get; set; }
    /// <summary>
    /// distance from the enemy that the unit will start charging
    /// </summary>
    public string ChargeDist { get; set; }
    /// <summary>
    /// delay between volleys, on top of the animation delay
    /// </summary>
    public string FireDelay { get; set; }
    /// <summary>
    /// no longer used
    /// </summary>
    public string[] Food { get; set; }
    /// <summary>
    /// [0] number of turns to build
    /// [1] cost to construct
    /// [2] cost of upkeep
    /// [3] cost of weapon upgrades
    /// [4] cost of armour upgrades
    /// [5] cost in custom battles
    /// </summary>
    public string[] Cost { get; set; }
    /// <summary>
    /// factions that can may have this unit
    /// </summary>
    public string[] Ownership { get; set; }
    /// <summary>
    /// a score given to the unit (randomiser use)
    /// </summary>
    public float PointsValue { get; set; }

    public Vector2 BattleSimScore { get; set; } = Vector2.Zero;

    public float PriScore { get; set; }
    public float SecScore { get; set; }

    public Unit() { }


    public void CalculatePointValue()
    {
        float points = 0f;
        float finalMod = 1f;

        Dictionary<string, float> weights = new()
        {
            {"health", 3f }, {"attackFactor", 1f }, {"attackBonus", 0.2f }, {"attackDelay", 1f},
            {"siegeAttack", 0.2f}, {"missileRange", 0.05f}, {"soldierNumber", 1f},
            {"armourFactor", 1.5f}, {"armourShield", 0.5f}, {"armourDefence", 1.2f}, {"morale", 1.2f},
            {"berserker", 10f}, {"impetuous", 0.6f}, {"disciplined", 1f}, {"low", 0.2f}, {"normal", 1f},
            {"highlyTrained", 4f}, {"trained", 2f}, {"untrained", 1f}, {"frightenFoot", 3f},
            {"frightenMounted", 3f}, {"command", 3f}, {"launching", 15f}, {"area", 10f},
            {"ap", 1.1f}, {"bp", 1.1f}, {"long_pike", 5f }, {"phalanx", 10f }
        };

        float weaponAttribute = this.CalcWeaponAttributes(weights, ref finalMod);
        float attributesVal = this.CalcAttributes(weights, ref finalMod);
        float mentalality = this.CalcMentality(weights, ref finalMod);
        float weapons = this.CalcWeaponStats(weights, ref finalMod);
        float armour = this.CalcArmour(weights, ref finalMod);
        float health = this.CalcHealth(weights, ref finalMod);
        float formation = this.CalcFormation(weights, ref finalMod);


        points = (health + armour + weaponAttribute +
            attributesVal + formation + mentalality + weapons) * finalMod;

        this.PointsValue = points / 10f;
    }

    public Vector2 BattleSim(Unit b)
    {
        const double const1 = 1;
        double dlf = 1.0;
        double mdf = 0.0;

        Dictionary<string, double> sStats = new()
        {
            {"attk", Convert.ToDouble(this.PriWep[0])}, {"defence", Convert.ToDouble(this.PriArm[0]) + Convert.ToDouble(this.PriArm[1]) + Convert.ToDouble(this.PriArm[2])},
            {"ap", this.PriAttri.Contains("ap") ? 1.0: 0.0}, {"range",  Convert.ToInt16(this.PriWep[3]) / 2}, {"lethality", Convert.ToDouble(this.PriWep[10]) },
            {"sec_attk", Convert.ToDouble(this.SecWep[0])}, {"sec_ap", this.SecAttri.Contains("ap") ? 1.0: 0.0}, {"sec_range",  Convert.ToInt16(this.SecWep[3]) / 2},
            {"sec_lethality", Convert.ToDouble(this.SecWep[10]) }, {"sec_defence", Convert.ToDouble(this.SecArmr[0]) + Convert.ToDouble(this.SecArmr[1])}
        };
        Dictionary<string, double> bstats = new()
        {
            {"attack", Convert.ToDouble(b.PriWep[0])}, {"defence", Convert.ToDouble(b.PriArm[0]) + Convert.ToDouble(b.PriArm[1]) + Convert.ToDouble(b.PriArm[2])},
            {"ap", b.PriAttri.Contains("ap") ? 1.0: 0.0}, {"range",  Convert.ToInt16(b.PriWep[3]) / 2}, {"lethality", Convert.ToDouble(b.PriWep[10]) },
            {"sec_attk", Convert.ToDouble(b.SecWep[0])}, {"sec_ap", b.SecAttri.Contains("ap") ? 1.0: 0.0}, {"sec_range",  Convert.ToInt16(b.SecWep[3]) / 2},
            {"sec_lethality", Convert.ToDouble(b.SecWep[10]) }, {"sec_defence", Convert.ToDouble(this.SecArmr[0]) + Convert.ToDouble(this.SecArmr[1])}
        };
        // ctk = chance to kill
        bool priIsMissile = false;
        bool secIsMissile = false;

        if (this.PriWep[5] is not "melee" and not "no")
        {
            priIsMissile = true;
        }

        if (this.SecWep[5] is not "melee" and not "no")
        {
            secIsMissile = true;
        }

        double oppDefPri;
        double oppDefSec;
        double ctk_pri_vs_pri = 0;
        double ctk_sec_vs_sec = 0;

        if (sStats["ap"] == 1.0)
        {
            oppDefPri = (bstats["defence"] + bstats["sec_defence"]) * 0.5;
        }
        else
        {
            oppDefPri = bstats["defence"] + bstats["sec_defence"];
        }

        if (sStats["sec_ap"] == 1.0)
        {
            oppDefSec = (bstats["defence"] + bstats["sec_defence"]) * 0.5;
        }
        else
        {
            oppDefSec = bstats["defence"] + bstats["sec_defence"];
        }

        if (priIsMissile)
        {
            ctk_pri_vs_pri = const1 * (sStats["attk"] - oppDefPri - (sStats["range"] / 10) + mdf + (dlf * 10));
        }
        else
        {
            ctk_pri_vs_pri = dlf * const1 * sStats["lethality"] * Math.Pow(1.1, sStats["attk"] - oppDefPri + mdf);
        }

        if (secIsMissile)
        {
            ctk_sec_vs_sec = const1 * (sStats["sec_attk"] - oppDefSec - (sStats["sec_range"] / 10) + mdf + (dlf * 10));
        }
        else
        {
            ctk_sec_vs_sec = dlf * const1 * sStats["sec_lethality"] * Math.Pow(1.1, sStats["sec_attk"] - oppDefSec + mdf);
        }

        if (ctk_sec_vs_sec < 0)
        {
            ctk_sec_vs_sec = 0;
        }

        if (ctk_pri_vs_pri < 0)
        {
            ctk_pri_vs_pri = 0;
        }

        ctk_pri_vs_pri *= 0.1;
        ctk_sec_vs_sec *= 0.1;

        return new Vector2((float)ctk_pri_vs_pri, (float)ctk_sec_vs_sec);
    }

    private float CalcFormation(Dictionary<string, float> weights, ref float finalmod)
    {
        float phalanx = 1f;

        if (this.Formation.Contains("phalanx"))
        {
            phalanx *= weights["phalanx"];
        }
        else
        {
            phalanx = 0f;
        }

        finalmod += phalanx * 0.2f;

        return phalanx;
    }

    private float CalcHealth(Dictionary<string, float> weights, ref float finalmod)
    {
        float h = Convert.ToInt16(this.Health[0]) * weights["health"];

        finalmod += h * 0.2f;

        return h;
    }

    private float CalcWeaponStats(Dictionary<string, float> weights, ref float finalmod)
    {
        float attk = Convert.ToInt16(this.PriWep[0]) * weights["attackFactor"];
        float charg = Convert.ToInt16(this.PriWep[1]) * weights["attackBonus"];
        float secAttk = Convert.ToInt16(this.SecWep[0]) * weights["attackFactor"];
        float secCharge = Convert.ToInt16(this.SecWep[1]) * weights["attackBonus"];

        string isRange = this.PriWep[2];
        float range = 1f;

        //if (isRange != "no")
        //attk = CalcRangedWeapon(weights, ref finalmod);



        float[] values = new float[] { attk, charg, secAttk, secCharge };

        this.AdjustFinalMod(ref finalmod, values, 0.05f);

        return attk + charg + secAttk + secCharge;
    }

    /*private float CalcRangedWeapon(Dictionary<string, float> weights, ref float finalmod)
    {
        var missileRange = Convert.ToInt16(priWep[3]) * weights["missileRange"];
        var attk = Convert.ToInt16(priWep[0]) * weights["attackFactor"];
        var soldiers = Convert.ToInt16(soldier[1]);
        //float expectedHitRate =


       // float value = attk

    }*/

    private float CalcWeaponAttributes(Dictionary<string, float> weights, ref float finalmod)
    {
        float priap = 1f;
        float pribp = 1f;
        float secap = 1f;
        float secbp = 1f;
        float longPike = 1f;
        float area = 1f;
        float launch = 1f;

        if (this.PriAttri.Contains("bp"))
        {
            pribp *= weights["bp"];
        }
        else
        {
            pribp = 0f;
        }

        if (this.PriAttri.Contains("ap"))
        {
            priap *= weights["ap"];
        }
        else
        {
            priap = 0;
        }

        if (this.SecAttri.Contains("bp"))
        {
            secbp *= weights["bp"];
        }
        else
        {
            secbp = 0f;
        }

        if (this.SecAttri.Contains("ap"))
        {
            secap *= weights["ap"];
        }
        else
        {
            secap = 0f;
        }

        if (this.SecAttri.Contains("launching"))
        {
            launch *= weights["launching"];
        }
        else
        {
            launch = 0f;
        }

        if (this.SecAttri.Contains("area"))
        {
            area *= weights["area"];
        }
        else
        {
            area = 0f;
        }

        if (this.PriAttri.Contains("long_pike"))
        {
            longPike *= weights["long_pike"];
        }
        else
        {
            longPike = 0f;
        }

        float[] values = new float[] { priap, pribp, area, secap, secbp, longPike, launch };

        this.AdjustFinalMod(ref finalmod, values, 0.3f);


        return priap + pribp + area + secap + secbp + longPike + launch;
    }

    private float CalcAttributes(Dictionary<string, float> weights, ref float finalmod)
    {
        float frightenFoot = 1f;
        float frightenMounted = 1f;
        float command = 1f;
        if (this.Attributes.Contains("frighten_foot"))
        {
            frightenFoot *= weights["frightenFoot"];
        }
        else
        {
            frightenFoot = 0f;
        }

        if (this.Attributes.Contains("frighten_mounted"))
        {
            frightenMounted *= weights["frightenMounted"];
        }
        else
        {
            frightenMounted = 0f;
        }

        if (this.Attributes.Contains("command"))
        {
            command *= weights["command"];
        }
        else
        {
            command = 0f;
        }

        float[] values = new float[] { frightenFoot, frightenMounted, command };

        this.AdjustFinalMod(ref finalmod, values, 0.2f);

        return frightenFoot + frightenMounted + command;
    }

    private float CalcMentality(Dictionary<string, float> weights, ref float finalmod)
    {
        float discipline = 1f;
        float training = 1f;
        float moral = Convert.ToInt16(this.Mental[0]) * weights["morale"];

        switch (this.Mental[1])
        {
            case "normal":
                discipline *= weights["normal"];
                break;
            case "low":
                discipline *= weights["low"];
                break;
            case "impetuous":
                discipline *= weights["impetuous"];
                break;
            case "disciplined":
                discipline *= weights["disciplined"];
                break;
            case "berserker":
                discipline *= weights["berserker"];
                break;
            default:
                break;
        }

        switch (this.Mental[2])
        {
            case "trained":
                discipline = training * weights["trained"];
                break;
            case "untrained":
                discipline = training * weights["untrained"];
                break;
            case "highly_trained":
                discipline = training * weights["highlyTrained"];
                break;
            default:
                break;
        }

        finalmod += discipline * 0.1f;
        finalmod += training * 0.1f;

        return training + discipline + moral;
    }

    private float CalcArmour(Dictionary<string, float> weights, ref float finalmod)
    {
        float armourF = Convert.ToInt16(this.PriArm[0]) * weights["armourFactor"];
        float armourD = Convert.ToInt16(this.PriArm[1]) * weights["armourDefence"];
        float armourS = Convert.ToInt16(this.PriArm[2]) * weights["armourShield"];

        float[] values = new float[] { armourF, armourD, armourS };

        this.AdjustFinalMod(ref finalmod, values, 0.05f);

        return armourF + armourD + armourS;
    }

    private void AdjustFinalMod(ref float finalmod, float[] values, float scale = 0.1f)
    {
        foreach (float val in values)
        {
            if (val > 0f)
            {
                finalmod += val * scale;
            }
        }
    }

    public string CompareTo([AllowNull] Unit other) => throw new NotImplementedException();

    int IComparable<Unit>.CompareTo(Unit other) => throw new NotImplementedException();

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is null)
        {
            return false;
        }

        throw new NotImplementedException();
    }

    public override int GetHashCode() => throw new NotImplementedException();

    public static bool operator ==(Unit left, Unit right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    public static bool operator !=(Unit left, Unit right) => !(left.Dic == right.Dic);

    public static bool operator <(Unit left, Unit right) => left is null ? right is not null : left.PriScore < right.PriScore;

    public static bool operator <=(Unit left, Unit right) => left is null || left.PriScore <= right.PriScore;

    public static bool operator >(Unit left, Unit right) => left is not null && left.PriScore > right.PriScore;

    public static bool operator >=(Unit left, Unit right) => left is null ? right is null : left.PriScore >= right.PriScore;
}
