namespace EMS.Staff.Domain;

public sealed class History
{
    public int Id { get; init; }
    public int PersonId { get; init; }
    public int? MentorId { get; init; }
    public DateTime CreatedOn { get; init; }
    public int Employment { get; init; }
    public int StaffId { get; init; }
    public Staff? Staff { get; init; }
}