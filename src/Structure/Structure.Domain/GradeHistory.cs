namespace EMS.Structure.Models;

public sealed class GradeHistory
{
    public int Id { get; init; }
    public DateTime CreatedOn { get; init; }
    public decimal Value { get; init; }
    public Grade? Grade { get; init; }
    public int GradeId { get; init; }
}