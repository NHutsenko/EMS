namespace EMS.Person.Interfaces;

public interface IUpdatePersonDataRepository
{
    Task UpdateNamesAsync(int id, string? lastName, string firstName, CancellationToken cancellationToken);
    Task UpdateGenderAsync(int id, bool gender, CancellationToken cancellationToken);
    Task UpdateAddressAsync(int personId, string city, string street, string building, string house, CancellationToken cancellationToken);
    Task UpdateAboutYourselfAsync(int id, string about, CancellationToken cancellationToken);
}