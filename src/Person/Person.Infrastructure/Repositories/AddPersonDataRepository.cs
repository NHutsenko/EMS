using EMS.Exceptions;
using EMS.Person.Application.Interfaces;
using EMS.Person.Domain;
using EMS.Person.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EMS.Person.Infrastructure.Repositories;

public sealed class AddPersonDataRepository: IAddPersonDataRepository
{
    private readonly PersonContext _context;

    public AddPersonDataRepository(PersonContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(PersonInfo info, CancellationToken cancellationToken)
    {
        if (await _context.People.AnyAsync(e => e.Login == info.Login, cancellationToken))
            throw new AlreadyExistsException($"Person with login {info.Login} already exists");

        await _context.People.AddAsync(info, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return info.Id;
    }

    public async Task<int> AddAddressAsync(Address address, CancellationToken cancellationToken)
    {
        await ThrowExceptionIfPersonNotFound(address.PersonId, cancellationToken);
        await _context.Addresses.AddAsync(address, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return address.Id;
    }

    public async Task AddContactsAsync(IEnumerable<Contact> contacts, CancellationToken cancellationToken)
    {
        await ThrowExceptionIfPersonNotFound(contacts.First().PersonId, cancellationToken);
        await _context.Contacts.AddRangeAsync(contacts, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task ThrowExceptionIfPersonNotFound(int personId, CancellationToken cancellationToken)
    {
        if (await _context.People.AnyAsync(e => e.Id == personId, cancellationToken) is false)
            throw new NotFoundException($"Person with id {personId} not found");
    }
}