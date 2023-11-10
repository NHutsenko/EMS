using EMS.Exceptions;
using EMS.Person.Application.Interfaces;
using EMS.Person.Domain;
using EMS.Person.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EMS.Person.Infrastructure.Repositories;

public sealed class UpdatePersonDataRepository: IUpdatePersonDataRepository
{
    private readonly PersonContext _context;

    public UpdatePersonDataRepository(PersonContext context)
    {
        _context = context;
    }
    
    public async Task UpdateNamesAsync(int id, string? lastName, string firstName, CancellationToken cancellationToken)
    {
        PersonInfo person = await GetPersonAsync(id, cancellationToken);

        if (string.IsNullOrEmpty(lastName) is false)
            _context.Entry(person).Property(e => e.LastName).CurrentValue = lastName;
        
        if (string.IsNullOrEmpty(firstName) is false)
            _context.Entry(person).Property(e => e.FirstName).CurrentValue = firstName;
        
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateGenderAsync(int id, bool gender, CancellationToken cancellationToken)
    {
        PersonInfo person = await GetPersonAsync(id, cancellationToken);
        _context.Entry(person).Property(e => e.Gender).CurrentValue = gender;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAddressAsync(int personId, string city, string street, string building, string house, CancellationToken cancellationToken)
    {
        Address? address = await _context.Addresses.FirstOrDefaultAsync(e => e.PersonId == personId, cancellationToken);
        if (address is null)
        {
            throw new NotFoundException($"Adders for person with id {personId} not found");
        }
        
        _context.Entry(address).Property(e => e.City).CurrentValue = city;
        _context.Entry(address).Property(e => e.Street).CurrentValue = street;
        _context.Entry(address).Property(e => e.Building).CurrentValue = building;
        _context.Entry(address).Property(e => e.House).CurrentValue = house;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAboutYourselfAsync(int id, string about, CancellationToken cancellationToken)
    {
        PersonInfo person = await GetPersonAsync(id, cancellationToken);
        _context.Entry(person).Property(e => e.About).CurrentValue = about;

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<PersonInfo> GetPersonAsync(int id, CancellationToken cancellationToken)
    {
        PersonInfo? person = await _context.People.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (person is null)
            throw new NotFoundException($"Person with id {id} no found");

        return person;
    }
}