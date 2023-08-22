namespace EMS.Person.Models;

public sealed class PersonInfo
{
    public int Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public DateTime BornOn { get; init; }
    public required string Login { get; init; }
    public bool Gender { get; init; }
    public string? About { get; init; }
    
    public IEnumerable<Contact> Contacts { get; init; } = new HashSet<Contact>();
    public Address? Address { get; init; }
}