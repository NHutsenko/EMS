using EMS.Person.Application.Interfaces;
using EMS.Person.Domain;
using EMS.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace EMS.Person.Application.Services;

public sealed partial class PersonService
{
    private readonly IAddPersonDataRepository _addPersonDataRepository;
    public override async Task<Empty> AddContact(NewContactRequest request, ServerCallContext context)
    {
        Contact contact = new()
        {
            PersonId = request.PersonId,
            ContactTypeId = (int)request.Contact.Type,
            Value = request.Contact.Value
        };
        List<Contact> contacts = new()
        {
            contact
        };
        
        await _addPersonDataRepository.AddContactsAsync(contacts, context.CancellationToken);
        return new Empty();
    }

    public override async Task<Empty> AddAddress(AddressRequest request, ServerCallContext context)
    {
        Address address = new()
        {
            PersonId = request.PersonId,
            City = request.Address.City,
            Street = request.Address.Street,
            Building = request.Address.Building,
            House = request.Address.House
        };

        await _addPersonDataRepository.AddAddressAsync(address, context.CancellationToken);

        return new Empty();
    }

    public override async Task<Int32Value> Create(PersonData request, ServerCallContext context)
    {
        PersonInfo info = new()
        {
            LastName = request.General.LastName,
            FirstName = request.General.FirstName,
            Login = request.General.Login,
            Gender = request.General.Gender,
            BornOn = DateTime.SpecifyKind(request.General.BornOn.ToDateTime(), DateTimeKind.Utc),
            About = request.General.About
        };

        int id = await _addPersonDataRepository.CreateAsync(info, context.CancellationToken);
        
        if (request.Address is not null)
        {
            Address address = new ()
            {
                PersonId = id,
                City = request.Address.City,
                Street = request.Address.Street,
                Building = request.Address.Building,
                House = request.Address.House
            };
            await _addPersonDataRepository.AddAddressAsync(address, context.CancellationToken);
        }

        if (request.Contacts.Count > 0)
        {
            IEnumerable<Contact> contacts = request.Contacts.Select(c => new Contact
            {
                PersonId = id,
                ContactTypeId = (int)c.Type,
                Value = c.Value
            }).ToList();
            await _addPersonDataRepository.AddContactsAsync(contacts, context.CancellationToken);
        };
        
        return new Int32Value
        {
            Value = id
        };
    }
}