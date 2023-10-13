using RTWLibPlus.dataWrappers;
using RTWLibPlus.interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RTWLibPlus.helpers;
using System.Threading;
using System.Numerics;

namespace RTWLibPlus.data.unit
{
    public class UnitsWrapper
    {
        List<Unit> units = new List<Unit>();

        public UnitsWrapper(EDU edu) {

            var dictionarys = edu.GetItemsByIdent("dictionary");
            var soldier = edu.GetItemsByIdent("soldier");
            var costs = edu.GetItemsByIdent("stat_cost");
            var ownership = edu.GetItemsByIdent("ownership");
            var weapons = edu.GetItemsByIdent("stat_pri");
            var secWep = edu.GetItemsByIdent("stat_sec");
            var armour = edu.GetItemsByIdent("stat_pri_armour");
            var secArmour = edu.GetItemsByIdent("stat_sec_armour");
            var attributes = edu.GetItemsByIdent("attributes");
            var mental = edu.GetItemsByIdent("stat_mental");
            var health = edu.GetItemsByIdent("stat_health");
            var priAtrri = edu.GetItemsByIdent("stat_pri_attr");
            var secAttri = edu.GetItemsByIdent("stat_sec_attr");
            var formation = edu.GetItemsByIdent("formation");
            units.init(dictionarys.Count);
            for (int i = 0; i < dictionarys.Count; i++)
            {
                units[i].dic = dictionarys[i].Value;
                units[i].cost = costs[i].Value.Split(',').TrimAll();
                units[i].ownership = ownership[i].Value.Split(',').TrimAll();
                units[i].priWep = weapons[i].Value.Split(',').TrimAll();
                units[i].secWep = secWep[i].Value.Split(',').TrimAll() ;
                units[i].priArm = armour[i].Value.Split(',').TrimAll();
                units[i].secArmr = secArmour[i].Value.Split(",").TrimAll();
                units[i].attributes = attributes[i].Value.Split(',').TrimAll();
                units[i].mental = mental[i].Value.Split(',').TrimAll();
                units[i].health = health[i].Value.Split(',').TrimAll();
                units[i].priAttri = priAtrri[i].Value.Split(',').TrimAll();
                units[i].secAttri = secAttri[i].Value.Split(',').TrimAll();
                units[i].formation = formation[i].Value.Split(',').TrimAll();
                units[i].soldier = soldier[i].Value.Split(',').TrimAll();
            }
            CalculateUnitValueS();

        }

        public void CalculateUnitValueS()
        {
            foreach (Unit unit in units)
            {
                unit.CalculatePointValue();
            }

            foreach(Unit unit in units)
            {
                Vector2 battleScore = Vector2.Zero;

                foreach(Unit opp in units)
                {
                    if (unit.dic == opp.dic)
                        continue;

                    battleScore += unit.BattleSim(opp);
                }

                unit.battleSimScore = battleScore;
                unit.pri_score = battleScore.X;
                unit.sec_score = battleScore.Y;
            }

        }

        public Unit GetUnit(int index)
        {
            if (index >= 0 && index < units.Count)
                return units[index];
            else return null;
        }
    }
}
