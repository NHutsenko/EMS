using System.Diagnostics.CodeAnalysis;
using EMS.Person.Domain;
using EMS.Protos;
using Google.Protobuf.WellKnownTypes;
using Enum = System.Enum;

namespace EMS.Person.Application.Mappers;

[ExcludeFromCodeCoverage]
public static class PersonMapper
{
    public static PersonData MapToProto(this PersonInfo data)
    {
        return new PersonData
        {
            Id = data.Id,
            General = new GeneralInfo
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                Gender = data.Gender,
                Login = data.Login,
                About = data.About,
                BornOn = DateTime.SpecifyKind(data.BornOn, DateTimeKind.Utc).ToTimestamp()
            },
            Contacts =
            {
                MapContacts(data.Contacts)
            },
            Address = MapAddress(data.Address)
        };
    }

    private static IEnumerable<ContactData> MapContacts(IEnumerable<Contact> contacts)
    {
        return contacts.Select(c => new ContactData
        {
            Id = c.Id,
            Value = c.Value,
            Type = (ContactData.Types.ContactType)Enum.Parse(typeof(ContactData.Types.ContactType), c.ContactTypeId.ToString())
        });
    }

    private static AddressData? MapAddress(Address? address)
    {
        if (address is null)
        {
            return null;
        }

        return new AddressData
        {
            City = address.City,
            Street = address.Street,
            Building = address.Building,
            House = address.House
        };
    }
}