using EMS.Person.Models;

namespace EMS.Person.Interfaces;

public interface IAddPersonDataRepository
{
    Task<int> CreateAsync(PersonInfo info, CancellationToken cancellationToken);
    Task<int> AddAddressAsync(Address address, CancellationToken cancellationToken);
    Task AddContactsAsync(IEnumerable<Contact> contacts, CancellationToken cancellationToken);
}