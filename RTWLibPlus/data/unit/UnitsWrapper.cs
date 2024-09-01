namespace RTWLibPlus.data.unit;
using RTWLibPlus.dataWrappers;
using System.Collections.Generic;
using RTWLibPlus.helpers;
using System.Numerics;

public class UnitsWrapper
{
    private readonly List<Unit> units = [];

    public UnitsWrapper(EDU edu)
    {

        List<interfaces.IBaseObj> dictionarys = edu.GetItemsByIdent("dictionary");
        List<interfaces.IBaseObj> soldier = edu.GetItemsByIdent("soldier");
        List<interfaces.IBaseObj> costs = edu.GetItemsByIdent("stat_cost");
        List<interfaces.IBaseObj> ownership = edu.GetItemsByIdent("ownership");
        List<interfaces.IBaseObj> weapons = edu.GetItemsByIdent("stat_pri");
        List<interfaces.IBaseObj> secWep = edu.GetItemsByIdent("stat_sec");
        List<interfaces.IBaseObj> armour = edu.GetItemsByIdent("stat_pri_armour");
        List<interfaces.IBaseObj> secArmour = edu.GetItemsByIdent("stat_sec_armour");
        List<interfaces.IBaseObj> attributes = edu.GetItemsByIdent("attributes");
        List<interfaces.IBaseObj> mental = edu.GetItemsByIdent("stat_mental");
        List<interfaces.IBaseObj> health = edu.GetItemsByIdent("stat_health");
        List<interfaces.IBaseObj> priAtrri = edu.GetItemsByIdent("stat_pri_attr");
        List<interfaces.IBaseObj> secAttri = edu.GetItemsByIdent("stat_sec_attr");
        List<interfaces.IBaseObj> formation = edu.GetItemsByIdent("formation");
        this.units.Init(dictionarys.Count);
        for (int i = 0; i < dictionarys.Count; i++)
        {
            this.units[i].Dic = dictionarys[i].Value;
            this.units[i].Cost = costs[i].Value.Split(',').TrimAll();
            this.units[i].Ownership = ownership[i].Value.Split(',').TrimAll();
            this.units[i].PriWep = weapons[i].Value.Split(',').TrimAll();
            this.units[i].SecWep = secWep[i].Value.Split(',').TrimAll();
            this.units[i].PriArm = armour[i].Value.Split(',').TrimAll();
            this.units[i].SecArmr = secArmour[i].Value.Split(",").TrimAll();
            this.units[i].Attributes = attributes[i].Value.Split(',').TrimAll();
            this.units[i].Mental = mental[i].Value.Split(',').TrimAll();
            this.units[i].Health = health[i].Value.Split(',').TrimAll();
            this.units[i].PriAttri = priAtrri[i].Value.Split(',').TrimAll();
            this.units[i].SecAttri = secAttri[i].Value.Split(',').TrimAll();
            this.units[i].Formation = formation[i].Value.Split(',').TrimAll();
            this.units[i].Soldier = soldier[i].Value.Split(',').TrimAll();
        }
        this.CalculateUnitValueS();

    }

    public void CalculateUnitValueS()
    {
        foreach (Unit unit in this.units)
        {
            unit.CalculatePointValue();
        }

        foreach (Unit unit in this.units)
        {
            Vector2 battleScore = Vector2.Zero;

            foreach (Unit opp in this.units)
            {
                if (unit.Dic == opp.Dic)
                {
                    continue;
                }

                battleScore += unit.BattleSim(opp);
            }

            unit.BattleSimScore = battleScore;
            unit.PriScore = battleScore.X;
            unit.SecScore = battleScore.Y;
        }

    }

    public Unit GetUnit(int index)
    {
        if (index >= 0 && index < this.units.Count)
        {
            return this.units[index];
        }
        else
        {
            return null;
        }
    }
}
