namespace RTWLibPlus.data.unit.unit_data;

public class Formation
{
    /// <summary>
    /// [0] soldier spacing (in meters) side to side
    /// [1] soldier spacing back to back
    /// </summary>
    private readonly float[] formationTight;
    /// <summary>
    /// [0] soldier spacing (in meters) side to side
    /// [1] soldier spacing back to back
    /// </summary>
    private readonly float[] formationSparse;
    /// <summary>
    /// number of ranks in the formation
    /// </summary>
    private readonly int formationRanks;
    /// <summary>
    /// special formations that are possible. one or two of square, horde, phalanx, testudo or wedge
    /// </summary>
    private readonly string formations;

    public Formation()
    {
        this.formationTight = new float[2];
        this.formationSparse = new float[2];
    }
}
