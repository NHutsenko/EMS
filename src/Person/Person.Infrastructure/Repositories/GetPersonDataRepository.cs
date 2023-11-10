using EMS.Exceptions;
using EMS.Person.Application.Interfaces;
using EMS.Person.Domain;
using EMS.Person.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EMS.Person.Infrastructure.Repositories;

public sealed class GetPersonDataRepository: IGetPersonDataRepository
{
    private readonly PersonContext _context;

    public GetPersonDataRepository(PersonContext context)
    {
        _context = context;
    }

    public async Task<PersonInfo> GetPersonAsync(int personId, CancellationToken cancellationToken)
    {
        IQueryable<PersonInfo> filtered = _context.People.Where(e => e.Id == personId);
        PersonInfo? data = await InculdeAddressAndContacts(filtered)
            .FirstOrDefaultAsync(cancellationToken);

        if (data is null)
            throw new NotFoundException($"Person with id {personId} not found");

        return data;
    }

    public async Task<IEnumerable<PersonInfo>> GetPeopleAsync(CancellationToken cancellationToken)
    {
        IQueryable<PersonInfo> filtered = _context.People.AsQueryable();
        IEnumerable<PersonInfo> data = await InculdeAddressAndContacts(filtered)
            .ToListAsync(cancellationToken);
        return data;
    }
    
    private IIncludableQueryable<PersonInfo, IEnumerable<Contact>?> InculdeAddressAndContacts(IQueryable<PersonInfo> request)
    {
        return request
            .Include(e => e.Address)
            .Include(e => e.Contacts);
    }
}