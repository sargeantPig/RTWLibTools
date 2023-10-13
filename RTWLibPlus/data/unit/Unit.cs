using Microsoft.VisualBasic;
using RTWLibPlus.data.unit.unit_data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace RTWLibPlus.data.unit
{
    public class Unit : IComparable<Unit>
    {
        /// <summary>
        /// stringernal name of the unit.
        /// </summary>
        public string type;
        /// <summary>
        /// tag for looking up the screen name
        /// </summary>
        public string dic;
        /// <summary>
        /// infantry, cavalry, siege, handler, ship or non_combatant
        /// </summary>
        public string category;
        /// <summary>
        /// light, heavy, spearman, or missle
        /// </summary>
        public string uClass;
        /// <summary>
        /// determines the type of voice used by the unit
        /// </summary>
        public string voice;
        /// <summary>
        /// make-up of each soldier (name, number, collision etc)
        /// </summary>
        public string[] soldier;
        /// <summary>
        /// name of officer model. There may be up to 0-3 officer lines per unit
        /// </summary>
        public string[] officer;
        /// <summary>
        /// type of siege used ( ballista, scorpion, onager, heavy_onager, repeating_ballista)
        /// </summary>
        public string engine;
        /// <summary>
        /// the type of non-ridden on animals used by the  wardogs, pigs
        /// </summary>
        public string animal;
        /// <summary>
        /// type of animal or vehicle ridden on
        /// </summary>
        public string mount;
        /// <summary>
        /// type of ship used if applicable
        /// </summary>
        public string naval;
        /// <summary>
        /// modifiers vs different mounts
        /// </summary>
        public MountEffect mountEffect;
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
        public string[] attributes;
        /// <summary>
        /// formation values
        /// </summary>
        public string[] formation;
        /// <summary>
        /// [0] hit postring of man
        /// [1] hitpostring of mount or attached animal (horses and camels do not have separate hitpostrings.
        /// </summary>
        public string[] health;
        /// <summary>
        /// primary weapon stats
        /// </summary>
        public string[] priWep;
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
        public string[] priAttri;
        /// <summary>
        /// secondary weapon stats
        /// </summary>
        public string[] secWep;
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
        public string[] secAttri;
        /// <summary>
        /// details of the units defences
        /// </summary>
        public string[] priArm;
        /// <summary>
        /// Details of animal's or vehicle's defenses (note riden horses do not have a separate defence)
        /// </summary>
        public string[] secArmr;
        /// <summary>
        /// Extra fatigue suffered by the unit in hot climates
        /// </summary>
        public string heat;
        /// <summary>
        /// combat modifiers for different ground types
        /// [0] scrub modifier
        /// [1] sand modifier
        /// [2] forest modifier
        /// [3] snow modifier
        /// </summary>
        public string[] ground;
        /// <summary>
        /// mental stats of the unit
        /// </summary>
        public string[] mental;
        /// <summary>
        /// distance from the enemy that the unit will start charging
        /// </summary>
        public string chargeDist;
        /// <summary>
        /// delay between volleys, on top of the animation delay
        /// </summary>
        public string fireDelay;
        /// <summary>
        /// no longer used
        /// </summary>
        public string[] food;
        /// <summary>
        /// [0] number of turns to build
        /// [1] cost to construct
        /// [2] cost of upkeep
        /// [3] cost of weapon upgrades
        /// [4] cost of armour upgrades
        /// [5] cost in custom battles
        /// </summary>
        public string[] cost;
        /// <summary>
        /// factions that can may have this unit
        /// </summary>
        public string[] ownership;
        /// <summary>
        /// a score given to the unit (randomiser use)
        /// </summary>
        public float points_value = 0;

        public Vector2 battleSimScore = Vector2.Zero;

        public float pri_score = 0.0f;
        public float sec_score = 0.0f;

        public Unit() { }


        public void CalculatePointValue()
        {
            float points = 0f;
            var finalMod = 1f;

            Dictionary<string, float> weights = new Dictionary<string, float>()
            {
                {"health", 3f }, {"attackFactor", 1f }, {"attackBonus", 0.2f }, {"attackDelay", 1f},
                {"siegeAttack", 0.2f}, {"missileRange", 0.05f}, {"soldierNumber", 1f},
                {"armourFactor", 1.5f}, {"armourShield", 0.5f}, {"armourDefence", 1.2f}, {"morale", 1.2f},
                {"berserker", 10f}, {"impetuous", 0.6f}, {"disciplined", 1f}, {"low", 0.2f}, {"normal", 1f},
                {"highlyTrained", 4f}, {"trained", 2f}, {"untrained", 1f}, {"frightenFoot", 3f},
                {"frightenMounted", 3f}, {"command", 3f}, {"launching", 15f}, {"area", 10f},
                {"ap", 1.1f}, {"bp", 1.1f}, {"long_pike", 5f }, {"phalanx", 10f }
            };

            float weaponAttribute = CalcWeaponAttributes(weights, ref finalMod);
            float attributesVal = CalcAttributes(weights, ref finalMod);
            float mentalality = CalcMentality(weights, ref finalMod);
            float weapons = CalcWeaponStats(weights, ref finalMod);
            float armour = CalcArmour(weights, ref finalMod);
            float health = CalcHealth(weights, ref finalMod);
            float formation = CalcFormation(weights, ref finalMod);
            

            points = (health + armour + weaponAttribute + 
                attributesVal + formation + mentalality + weapons) * finalMod;

            points_value = points / 10f;
        }

        public Vector2 BattleSim(Unit b)
        {
            const double const1 = 1;
            double dlf = 1.0;
            double mdf = 0.0;

            Dictionary<string, double> sStats = new Dictionary<string, double>()
            {
                {"attk", Convert.ToDouble(this.priWep[0])}, {"defence", Convert.ToDouble(this.priArm[0]) + Convert.ToDouble(this.priArm[1]) + Convert.ToDouble(this.priArm[2])},
                {"ap", this.priAttri.Contains("ap") ? 1.0: 0.0}, {"range",  Convert.ToInt16(this.priWep[3]) / 2}, {"lethality", Convert.ToDouble(this.priWep[10]) },
                {"sec_attk", Convert.ToDouble(this.secWep[0])}, {"sec_ap", this.secAttri.Contains("ap") ? 1.0: 0.0}, {"sec_range",  Convert.ToInt16(this.secWep[3]) / 2}, 
                {"sec_lethality", Convert.ToDouble(this.secWep[10]) }, {"sec_defence", Convert.ToDouble(this.secArmr[0]) + Convert.ToDouble(this.secArmr[1])}
            };
            Dictionary<string, double> bstats = new Dictionary<string, double>()
            {
                {"attack", Convert.ToDouble(b.priWep[0])}, {"defence", Convert.ToDouble(b.priArm[0]) + Convert.ToDouble(b.priArm[1]) + Convert.ToDouble(b.priArm[2])},
                {"ap", b.priAttri.Contains("ap") ? 1.0: 0.0}, {"range",  Convert.ToInt16(b.priWep[3]) / 2}, {"lethality", Convert.ToDouble(b.priWep[10]) },
                {"sec_attk", Convert.ToDouble(b.secWep[0])}, {"sec_ap", b.secAttri.Contains("ap") ? 1.0: 0.0}, {"sec_range",  Convert.ToInt16(b.secWep[3]) / 2},
                {"sec_lethality", Convert.ToDouble(b.secWep[10]) }, {"sec_defence", Convert.ToDouble(this.secArmr[0]) + Convert.ToDouble(this.secArmr[1])}
            };
            // ctk = chance to kill
            bool priIsMissile = false;
            bool secIsMissile = false;

            if (priWep[5] != "melee" && priWep[5] != "no")
                priIsMissile = true;
            if (secWep[5] != "melee" && secWep[5] != "no")
                secIsMissile = true;

            double oppDefPri;
            double oppDefSec;
            double ctk_pri_vs_pri = 0;
            double ctk_sec_vs_sec = 0;

            if (sStats["ap"] == 1.0)
                oppDefPri = (bstats["defence"] + bstats["sec_defence"]) * 0.5;
            else oppDefPri = bstats["defence"] + bstats["sec_defence"];
            if (sStats["sec_ap"] == 1.0)
                oppDefSec = (bstats["defence"] + bstats["sec_defence"]) * 0.5;
            else oppDefSec = (bstats["defence"] + bstats["sec_defence"]);

            if (priIsMissile)
                ctk_pri_vs_pri = const1 * (((sStats["attk"] - oppDefPri - (sStats["range"] / 10))) + mdf + (dlf * 10));
            else ctk_pri_vs_pri = dlf * const1 *sStats["lethality"] * Math.Pow(1.1, (sStats["attk"] - oppDefPri + mdf));
            if (secIsMissile)
                ctk_sec_vs_sec = const1 * (((sStats["sec_attk"] - oppDefSec - (sStats["sec_range"] / 10))) + mdf + (dlf * 10));
            else ctk_sec_vs_sec = dlf * const1 * sStats["sec_lethality"] * Math.Pow(1.1, (sStats["sec_attk"] - oppDefSec + mdf));

            if (ctk_sec_vs_sec < 0)
                ctk_sec_vs_sec = 0;
            if (ctk_pri_vs_pri < 0)
                ctk_pri_vs_pri = 0;

            ctk_pri_vs_pri = ctk_pri_vs_pri * 0.1;
            ctk_sec_vs_sec = ctk_sec_vs_sec * 0.1;

            return new Vector2((float)ctk_pri_vs_pri, (float)ctk_sec_vs_sec);
        }

        private float CalcFormation(Dictionary<string, float> weights, ref float finalmod)
        {
            float phalanx = 1f;

            if (formation.Contains("phalanx"))
                phalanx = phalanx * weights["phalanx"];
            else phalanx = 0f;

            finalmod += (phalanx * 0.2f);

            return phalanx;
        }

        private float CalcHealth(Dictionary<string, float> weights, ref float finalmod)
        {
            var h = Convert.ToInt16(health[0]) * weights["health"];

            finalmod += (h * 0.2f);

            return h;
        }

        private float CalcWeaponStats(Dictionary<string, float> weights, ref float finalmod)
        {
            var attk = Convert.ToInt16(priWep[0]) * weights["attackFactor"];
            var charg = Convert.ToInt16(priWep[1]) * weights["attackBonus"];
            var secAttk = Convert.ToInt16(secWep[0]) * weights["attackFactor"];
            var secCharge = Convert.ToInt16(secWep[1]) * weights["attackBonus"];
            
            var isRange = priWep[2];
            float range = 1f;

            //if (isRange != "no")
                //attk = CalcRangedWeapon(weights, ref finalmod);



            float[] values = new float[] { attk, charg, secAttk, secCharge };

            AdjustFinalMod(ref finalmod, values, 0.05f);

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

            if (priAttri.Contains("bp"))
                pribp = pribp * weights["bp"];
            else pribp = 0f;
            if (priAttri.Contains("ap"))
                priap = priap * weights["ap"];
            else priap = 0;
            if (secAttri.Contains("bp"))
                secbp = secbp * weights["bp"];
            else secbp = 0f;
            if (secAttri.Contains("ap"))
                secap = secap * weights["ap"];
            else secap = 0f;
            if (secAttri.Contains("launching"))
                launch = launch * weights["launching"];
            else launch = 0f;
            if (secAttri.Contains("area"))
                area = area * weights["area"];
            else area = 0f;
            if (priAttri.Contains("long_pike"))
                longPike = longPike * weights["long_pike"];
            else longPike = 0f;

            float[] values = new float[] {priap, pribp, area, secap, secbp, longPike, launch };

            AdjustFinalMod(ref finalmod, values, 0.3f);


            return priap + pribp + area + secap + secbp + longPike + launch;
        }

        private float CalcAttributes(Dictionary<string, float> weights, ref float finalmod)
        {
            float frightenFoot = 1f;
            float frightenMounted = 1f;
            float command = 1f;
            if (attributes.Contains("frighten_foot"))
                frightenFoot = frightenFoot * weights["frightenFoot"];
            else frightenFoot = 0f;
            if (attributes.Contains("frighten_mounted"))
                frightenMounted = frightenMounted * weights["frightenMounted"];
            else frightenMounted = 0f;
            if (attributes.Contains("command"))
                command = command * weights["command"];
            else command = 0f;

            float[] values = new float[] {frightenFoot, frightenMounted, command };

            AdjustFinalMod(ref finalmod, values, 0.2f);

            return frightenFoot + frightenMounted + command;
        }

        private float CalcMentality(Dictionary<string, float> weights, ref float finalmod)
        {
            float discipline = 1f;
            float training = 1f;
            var moral = Convert.ToInt16(mental[0]) * weights["morale"];

            switch (mental[1])
            {
                case "normal":
                    discipline = discipline * weights["normal"];
                    break;
                case "low":
                    discipline = discipline * weights["low"];
                    break;
                case "impetuous":
                    discipline = discipline * weights["impetuous"];
                    break;
                case "disciplined":
                    discipline = discipline * weights["disciplined"];
                    break;
                case "berserker":
                    discipline = discipline * weights["berserker"];
                    break;
            }

            switch (mental[2])
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
            }

            finalmod += (discipline * 0.1f);
            finalmod += (training * 0.1f);

            return training + discipline + moral;
        }

        private float CalcArmour(Dictionary<string, float> weights, ref float finalmod)
        {
            var armourF = Convert.ToInt16(priArm[0]) * weights["armourFactor"];
            var armourD = Convert.ToInt16(priArm[1]) * weights["armourDefence"];
            var armourS = Convert.ToInt16(priArm[2]) * weights["armourShield"];

            float[] values = new float[] {armourF, armourD, armourS};

            AdjustFinalMod(ref finalmod, values, 0.05f);

            return armourF + armourD + armourS;
        }

        private void AdjustFinalMod(ref float finalmod, float[] values, float scale = 0.1f)
        {
            foreach (var val in values)
            {
                if (val > 0f)
                    finalmod += (val * scale);
            }
        }

        public string CompareTo([AllowNull] Unit other)
        {
            throw new NotImplementedException();
        }

        int IComparable<Unit>.CompareTo(Unit other)
        {
            throw new NotImplementedException();
        }
    }
}
