using EMS.Person.Context;
using EMS.Person.Models;
using EMS.Protos;
using Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace EMS.Person.Services;

public sealed class PersonService: Protos.PersonService.PersonServiceBase
{
    private readonly PersonContext _dbContext;

    public PersonService(PersonContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<Empty> UpdateNames(NamesRequest request, ServerCallContext context)
    {
        PersonInfo info = await GetPersonAsync(request.Id, context.CancellationToken);

        if (string.IsNullOrWhiteSpace(request.FirstName) is false)
        {
            _dbContext.Entry(info).Property(e => e.FirstName).CurrentValue = request.FirstName;
        }

        if (string.IsNullOrWhiteSpace(request.LastName) is false)
        {
            _dbContext.Entry(info).Property(e => e.LastName).CurrentValue = request.LastName;
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);
        return new Empty();
    }

    public override async Task<Empty> UpdateGender(GenderRequest request, ServerCallContext context)
    {
        PersonInfo info = await GetPersonAsync(request.Id, context.CancellationToken);

        _dbContext.Entry(info).Property(e => e.Gender).CurrentValue = request.Value;
        
        await _dbContext.SaveChangesAsync(context.CancellationToken);
        return new Empty();
    }

    public override async Task<Empty> UpdateLogin(LoginRequest request, ServerCallContext context)
    {
        PersonInfo info = await GetPersonAsync(request.Id, context.CancellationToken);

        _dbContext.Entry(info).Property(e => e.Login).CurrentValue = request.Value;
        
        await _dbContext.SaveChangesAsync(context.CancellationToken);
        return new Empty();
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
}