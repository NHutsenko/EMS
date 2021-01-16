using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using EMS.Core.API.Models;
using EMS.Core.API.DAL.Repositories.Interfaces;
using System.Linq;
using System;

namespace EMS.Core.API.Services
{
    public class PeopleService : People.PeopleBase
    {
        private readonly IPeopleRepository _peopleRepository;

        public PeopleService(IPeopleRepository peopleRepository)
        {
            _peopleRepository = peopleRepository;
        }

        public override Task<PeopleResponse> GetAll(Empty request, ServerCallContext context)
        {
            PeopleResponse response = new PeopleResponse
            {
                Response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            IQueryable<Person> people = _peopleRepository.GetAll();

            foreach (Person person in people)
            {
                response.People.Add(ConvertData(person));
            }

            return Task.FromResult(response);
        }

        public override Task<PersonResponse> GetById(PersonRequest request, ServerCallContext context)
        {
            PersonResponse response = new PersonResponse
            {
                Response = new BaseResponse
                {
                    Code = Code.UnknownError,
                    ErrorMessage = string.Empty
                },
                Data = null
            };
            try
            {
                Person person = _peopleRepository.GetById(request.Id);
                PersonData data = ConvertData(person);
                response.Data = data;
                response.Response.Code = Code.Success;
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                response.Response.ErrorMessage = ex.Message;
                response.Response.Code = Code.DataError;
                return Task.FromResult(response);
            }
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

            if (person.Contacts is not null)
            {
                foreach (Contact contact in person.Contacts)
                {
                    converted.Contacts.Add(new ContactData
                    {
                        Name = contact.Name,
                        ContactType = (int)contact.ContactType,
                        PersonId = contact.PersonId,
                        Value = contact.Value
                    });
                }
            }

            if (person.Photos is not null)
            {
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
            }

            return converted;
        }
    }
}
