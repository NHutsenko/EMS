namespace EMS.Structure.Models;

public sealed class Member
{
    public int Id { get; init; }
    public int MemberId { get; init; }
    public DateTime StartWork { get; init; }
    public DateTime? EndWork { get; init; }
    public int Employment { get; init; }
    public Team? Team { get; init; }
    public int TeamId { get; init; }
}