using EMS.Person.Context;
using EMS.Person.Mappers;
using EMS.Person.Models;
using EMS.Protos;
using Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EMS.Person.Services;

public sealed partial class PersonService : Protos.PersonService.PersonServiceBase
{
    private readonly PersonContext _dbContext;

    public PersonService(PersonContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<PersonData?> Get(Int32Value request, ServerCallContext context)
    {
        IQueryable<PersonInfo> filtered = _dbContext.People.Where(e => e.Id == request.Value);
        PersonInfo? data = await GetPeopleData(filtered)
            .FirstOrDefaultAsync(context.CancellationToken);

        if (data is null)
        {
            throw new NotFoundException($"Person with id {request.Value} not found");
        }
        
        return data.MapToProto();
    }

    public override async Task<People> GetAll(Empty request, ServerCallContext context)
    {
        People people = new();

        IQueryable<PersonInfo> filtered = _dbContext.People.AsQueryable();
        IEnumerable<PersonData> data = (await GetPeopleData(filtered)
                .ToListAsync(context.CancellationToken))
            .Select(e => e.MapToProto());
        people.Data.AddRange(data);

        return people;
    }

    private async Task<PersonInfo> GetPersonAsync(int id, CancellationToken cancellationToken)
    {
        PersonInfo? info = await _dbContext.People.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (info is null)
        {
            throw new NotFoundException($"Person with id: {id} not found");
        }

        return info;
    }

    private IIncludableQueryable<PersonInfo, IEnumerable<Contact>?> GetPeopleData(IQueryable<PersonInfo> request)
    {
        return request
            .Include(e => e.Address)
            .Include(e => e.Contacts);
    }
}