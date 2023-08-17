namespace EMS.Person.Models;

public sealed record Address
{
    public int Id { get; init; }
    public string? City { get; init; }
    public string? Street { get; init; }
    public string? Building { get; init; }
    public string? House { get; init; }
    
    public PersonInfo? Person { get; init; }
    public int PersonId { get; init; }
}