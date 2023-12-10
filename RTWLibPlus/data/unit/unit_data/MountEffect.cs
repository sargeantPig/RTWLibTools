namespace RTWLibPlus.data.unit.unit_data;
using System.Collections.Generic;

public class MountEffect
{
    /// <summary>
    /// mount types that this unit has bonuses against
    /// </summary>
    private readonly List<string> mountType;
    /// <summary>
    /// the bonus or negative for each mount type
    /// </summary>
    private readonly List<int> modifier;

    public MountEffect()
    {
        this.mountType = new List<string>();
        this.modifier = new List<int>();
    }
}
