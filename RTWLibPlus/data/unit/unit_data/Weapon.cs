namespace RTWLibPlus.data.unit.unit_data;

public class StatWeapons
{
    /// <summary>
    /// [0] attack factor
    /// [1] attack bonus factor if charging
    /// </summary>
    private readonly int[] atk;
    /// <summary>
    /// missile type fired (no if not a missile weapon type)
    /// </summary>
	private readonly string missType;
    /// <summary>
    /// [0] range of missile
    /// [1] amount of missle ammunition per man
    /// </summary>
	private readonly int[] missAttr;
    /// <summary>
    ///  Weapon type = melee, thrown, missile, or siege_missile
    /// </summary>
	private readonly string wepFlags;
    /// <summary>
    /// Tech type = simple, other, blade, archery or siege
    /// </summary>
	private readonly string techFlags;
    /// <summary>
    /// Damage type = piercing, blunt, slashing or fire. (I don't think this is used anymore)
    /// </summary>
    private readonly string dmgFlags;
    /// <summary>
    /// Sound type when weapon hits = none, knife, mace, axe, sword, or spear
    /// </summary>
	private readonly string soundFlags;
    /// <summary>
    /// Min delay between attacks(in 1/10th of a second)
    /// </summary>
	private readonly float[] atkDly;

    public StatWeapons()
    {
        this.atk = new int[2];
        this.missAttr = new int[2];
        this.atkDly = new float[2];
    }
}
