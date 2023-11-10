namespace EMS.Structure.Position.Domain;

public sealed class Position
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public IEnumerable<Grade> Grades { get; init; } = new HashSet<Grade>();
}