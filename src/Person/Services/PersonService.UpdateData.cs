using EMS.Exceptions;
using EMS.Person.Models;
using EMS.Protos;
using Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace EMS.Person.Services;

public sealed partial class PersonService
{
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

    public override async Task<Empty> UpdateAddress(AddressRequest request, ServerCallContext context)
    {
        Address? address = await _dbContext.Addresses.FirstOrDefaultAsync(e => e.PersonId == request.PersonId, context.CancellationToken);
        if (address is null)
        {
            throw new NotFoundException($"Adders for person with id {request.PersonId} not found");
        }
        
        _dbContext.Entry(address).Property(e => e.City).CurrentValue = request.Address.City;
        _dbContext.Entry(address).Property(e => e.Street).CurrentValue = request.Address.Street;
        _dbContext.Entry(address).Property(e => e.Building).CurrentValue = request.Address.Building;
        _dbContext.Entry(address).Property(e => e.House).CurrentValue = request.Address.House;

        await _dbContext.SaveChangesAsync(context.CancellationToken);
        
        return new Empty();
    }

    public override async Task<Empty> UpdateAboutYourself(AboutRequest request, ServerCallContext context)
    {
        PersonInfo person = await GetPersonAsync(request.PersonId, context.CancellationToken);

        _dbContext.Entry(person).Property(e => e.About).CurrentValue = request.Value;
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new Empty();
    }
}