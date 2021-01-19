using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using EMS.Core.API.Models;
using EMS.Core.API.DAL.Repositories.Interfaces;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using EMS.Common.Protos;
using EMS.Core.API.Enums;

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
                response.Data.Add(ConvertData(person));
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
            catch (NullReferenceException nrex)
            {
                string error = $"Some data has not found (type: {nrex.GetType().Name})";
                response.Response.ErrorMessage = error;
                response.Response.Code = Code.DataError;
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                response.Response.ErrorMessage = ex.Message;
                response.Response.Code = Code.UnknownError;
                return Task.FromResult(response);
            }
        }

        public override async Task<BaseResponse> AddAsync(PersonData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _peopleRepository.AddAsync(null);

                Person person = new Person
                {
                    LastName = request.LastName,
                    Name = request.Name,
                    SecondName = request.SecondName,
                    BornedOn = request.BornedOn.ToDateTime()
                };
                int result = await _peopleRepository.AddAsync(person);
                if(result == 0)
                {
                    throw new Exception("Person data has not been saved");
                }
                return new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                };
            }
            catch (NullReferenceException nrex)
            {
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = nrex.Message
                };
            }
            catch (ArgumentException aex)
            {
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = aex.Message
                };
            }
            catch (DbUpdateException duex)
            {
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving person data"
                };
            }
            catch(Exception ex)
            {
                return new BaseResponse
                {
                    Code = Code.UnknownError,
                    ErrorMessage = ex.Message
                };
            }
        }

        public override async Task<BaseResponse> UpdateAsync(PersonData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _peopleRepository.UpdateAsync(null);

                Person person = new Person
                {
                    LastName = request.LastName,
                    Name = request.Name,
                    SecondName = request.SecondName,
                    BornedOn = request.BornedOn.ToDateTime(),
                    Id = request.Id
                };
                int result = await _peopleRepository.UpdateAsync(person);
                if (result == 0)
                {
                    throw new Exception("Person data has not been updated");
                }
                return new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                };
            }
            catch (NullReferenceException nrex)
            {
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = nrex.Message
                };
            }
            catch (ArgumentException aex)
            {
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = aex.Message
                };
            }
            catch (DbUpdateException duex)
            {
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving person data"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Code = Code.UnknownError,
                    ErrorMessage = ex.Message
                };
            }
        }

        public override async Task<BaseResponse> AddContactAsync(ContactData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _peopleRepository.AddContactAsync(null);

                Contact contact = new Contact
                {
                    ContactType = (ContactType)System.Enum.Parse(typeof(ContactType), request.ContactType.ToString()),
                    Name = request.Name,
                    PersonId = request.PersonId,
                    Value = request.Value
                };
                int result = await _peopleRepository.AddContactAsync(contact);
                if(result == 0)
                {
                    throw new Exception("Contact has not been saved");
                }
                return new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                };
            }
            catch(NullReferenceException nrex)
            {
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = nrex.Message
                };
            }
            catch(ArgumentException aex)
            {
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = aex.Message
                };
            }
            catch(DbUpdateException duex)
            {
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving contact"
                };
            }
            catch(Exception ex)
            {
                return new BaseResponse
                {
                    Code = Code.UnknownError,
                    ErrorMessage = ex.Message
                };
            }
        }

        public override async Task<BaseResponse> AddPhotoAsync(PhotoData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _peopleRepository.AddPhotoAsync(null);
                PersonPhoto photo = new PersonPhoto
                {
                    Base64 = request.Base64,
                    Name = request.Name,
                    PersonId = request.PersonId
                };
                int result = await _peopleRepository.AddPhotoAsync(photo);
                if(result == 0)
                {
                    throw new Exception("Photo has not been saved");
                }
                return new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                };
            }
            catch (NullReferenceException nrex)
            {
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = nrex.Message
                };
            }
            catch (ArgumentException aex)
            {
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = aex.Message
                };
            }
            catch (DbUpdateException duex)
            {
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving contact"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Code = Code.UnknownError,
                    ErrorMessage = ex.Message
                };
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
