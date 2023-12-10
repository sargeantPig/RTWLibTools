namespace RTWLibPlus.data.unit.unit_data;

public class Mentality
{
    /// <summary>
    /// Base morale
    /// </summary>
    private readonly int morale;
    /// <summary>
    /// discipline of the unit (normal, low, disciplined or impetuous)
    /// </summary>
    private readonly string discipline;
    /// <summary>
    /// training of the unit (how tidy the formation is)
    /// </summary>
    private readonly string training;

    public Mentality(int morale, string discipline, string training)
    {
        this.morale = morale;
        this.discipline = discipline;
        this.training = training;
    }
}
