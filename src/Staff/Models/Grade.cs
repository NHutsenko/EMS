namespace EMS.Staff.Models;

public sealed class Grade
{
    public int Id { get; init; }
    public int Value { get; init; }
    public IEnumerable<GradeHistory> History { get; init; } = new HashSet<GradeHistory>();
    public Position? Position { get; init; }
    public int PositionId { get; init; }
}