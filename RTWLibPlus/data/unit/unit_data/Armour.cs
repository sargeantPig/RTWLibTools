namespace RTWLibPlus.data.unit.unit_data;

public class StatPriArmour
{
    /// <summary>
    /// [1] armour factor
    /// [2] defensive skill factor
    /// [3] shield factor
    /// </summary>
    private readonly int[] priArm;
    /// <summary>
    /// sound type when hit = flesh, leather, or metal
    /// </summary>
    private readonly string armSound;

    public StatPriArmour() => this.priArm = new int[3];
}
