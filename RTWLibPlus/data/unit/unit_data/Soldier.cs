namespace RTWLibPlus.data.unit.unit_data;

public class Soldier
{
    /// <summary>
    /// name of the soldier model to use
    /// </summary>
    private readonly string name;
    /// <summary>
    /// number of ordinary soldiers in the unit
    /// </summary>
    private readonly int number;
    /// <summary>
    /// number of extras (pigs, dogs, elephants, chariots) attached to the unit
    /// </summary>
    private readonly int extras;
    /// <summary>
    /// collision mass of the men. 1.0 is normal. Only applies to infantry
    /// </summary>
    private readonly float collMass;

    public Soldier()
    {
    }
}
