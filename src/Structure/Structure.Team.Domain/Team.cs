namespace EMS.Structure.Team.Domain;

public sealed class Team
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public IEnumerable<Member> Members { get; init; } = new HashSet<Member>();
}