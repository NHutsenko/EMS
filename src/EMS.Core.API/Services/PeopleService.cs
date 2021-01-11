using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using System.Collections.Generic;
using EMS.Core.API.Models;
using EMS.Core.API.DAL.Repositories.Interfaces;
using System.Linq;
using Google.Protobuf.Collections;

namespace EMS.Core.API.Services
{
    public class PeopleService: People.PeopleBase
    {
        private readonly IPeopleRepository _peopleRepository;
        
        public PeopleService(IPeopleRepository peopleRepository)
        {
            _peopleRepository = peopleRepository;
        }

        public override Task<PeopleData> GetAll(Empty request, ServerCallContext context)
        {
            PeopleData response = new PeopleData();

            IQueryable<Person> people = _peopleRepository.GetAll();

            foreach(Person person in people)
            {
                response.People.Add(ConvertData(person));
            }

            return Task.FromResult(response);
        }

        public override Task<PersonData> GetById(PersonRequest request, ServerCallContext context)
        {
            Person person = _peopleRepository.GetById(request.Id);
            return Task.FromResult(ConvertData(person));
        }

        private static PersonData ConvertData(Person person)
        {
            PersonData converted = new PersonData
            {
                Id = person.Id,
                Name = person.Name,
                LastName = person.LastName,
                SecondName = person.SecondName,
                BornedOn = Timestamp.FromDateTime(person.BornedOn.ToUniversalTime()),
                CreatedOn = Timestamp.FromDateTime(person.CreatedOn.ToUniversalTime())
            };

            foreach(Contact contact in person.Contacts)
            {
                converted.Contacts.Add(new ContactData
                {
                    Name = contact.Name,
                    ContactType = (int)contact.ContactType,
                    PersonId = contact.PersonId,
                    Value = contact.Value
                });
            }

            foreach (PersonPhoto photo in person.Photos)
            {
                converted.Photos.Add(new PhotoData
                {
                    PersonId = photo.PersonId,
                    Base64 = photo.Base64,
                    Mime = photo.Mime,
                    Name = photo.Name
                });
            }

            return converted;
        }
    }
}
