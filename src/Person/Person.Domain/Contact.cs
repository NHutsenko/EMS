namespace EMS.Person.Domain;

public sealed class Contact
{
    public int Id { get; init; }
    public required string Value { get; init; }
    
    public int PersonId { get; init; }
    public PersonInfo? Person { get; init; }
    
    public int ContactTypeId { get; init; }
    public ContactType? ContactType { get; init; }
}