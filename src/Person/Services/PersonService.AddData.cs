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
    public override async Task<Int32Value> AddContact(NewContactRequest request, ServerCallContext context)
    {
        await GetPersonAsync(request.PersonId, context.CancellationToken);

        Contact contact = new()
        {
            PersonId = request.PersonId,
            ContactTypeId = (int)request.Contact.Type,
            Value = request.Contact.Value
        };
        await _dbContext.Contacts.AddAsync(contact, context.CancellationToken);
        await _dbContext.SaveChangesAsync(context.CancellationToken);
        return new Int32Value
        {
            Value = contact.Id
        };
    }

    public override async Task<Empty> AddAddress(AddressRequest request, ServerCallContext context)
    {
        PersonInfo person = await GetPersonAsync(request.PersonId, context.CancellationToken);

        Address address = new()
        {
            PersonId = person.Id,
            City = request.Address.City,
            Street = request.Address.Street,
            Building = request.Address.Building,
            House = request.Address.House
        };

        await _dbContext.Addresses.AddAsync(address, context.CancellationToken);
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new Empty();
    }

    public override async Task<Int32Value> Create(PersonData request, ServerCallContext context)
    {
        if (await _dbContext.People.AnyAsync(e => e.Login == request.General.Login, context.CancellationToken))
        {
            throw new AlreadyExistsException($"Person with login {request.General.Login} already exists");
        }

        PersonInfo info = new()
        {
            LastName = request.General.LastName,
            FirstName = request.General.FirstName,
            Login = request.General.Login,
            Gender = request.General.Gender,
            BornOn = DateTime.SpecifyKind(request.General.BornOn.ToDateTime(), DateTimeKind.Utc),
            About = request.General.About,
            Address = request.Address == null ? null : new Address
            {
                City = request.Address.City,
                Street = request.Address.Street,
                Building = request.Address.Building,
                House = request.Address.House
            },
            Contacts = request.Contacts.Select(c => new Contact
            {
                ContactTypeId = (int)c.Type,
                Value = c.Value
            }).ToList()
        };

        await _dbContext.People.AddAsync(info, context.CancellationToken);
        await _dbContext.SaveChangesAsync(context.CancellationToken);
        return new Int32Value
        {
            Value = info.Id
        };
    }
}