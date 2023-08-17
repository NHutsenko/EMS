namespace EMS.Person.Models;

public sealed record ContactType
{
    public int Id { get; init; }
    public required string Value { get; init; }
    public ICollection<Contact> Contacts { get; init; } = new HashSet<Contact>();
}