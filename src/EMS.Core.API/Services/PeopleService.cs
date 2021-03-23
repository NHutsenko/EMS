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
using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Common.Logger.Models;

namespace EMS.Core.API.Services
{
    public class PeopleService : People.PeopleBase
    {
        private readonly IPeopleRepository _peopleRepository;
        private readonly IEMSLogger<PeopleService> _logger;
        private readonly IDateTimeUtil _dateTimeUtil;

        public PeopleService(IPeopleRepository peopleRepository, IEMSLogger<PeopleService> logger, IDateTimeUtil dateTimeUtil)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _dateTimeUtil = dateTimeUtil;
        }

        public override Task<PeopleResponse> GetAll(Empty request, ServerCallContext context)
        {
            PeopleResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            try
            {
                IQueryable<Person> people = _peopleRepository.GetAll();

                foreach (Person person in people)
                {
                    response.Data.Add(ToRpcModel(person));
                }

                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(GetAll),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = response
				};
                _logger.AddLog(logData);
            }
            catch(Exception ex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(GetAll),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = ex
				};
                _logger.AddErrorLog(logData);
                response.Status.Code = Code.UnknownError;
                response.Status.ErrorMessage = "An error orccured while loading people data";
            }
            return Task.FromResult(response);
        }

        public override Task<PersonResponse> GetById(ByPersonIdRequest request, ServerCallContext context)
        {
            PersonResponse response = new()
			{
				Status = new BaseResponse { Code = Code.UnknownError, ErrorMessage = string.Empty },
				Data = null
			};
            try
            {
                Person person = _peopleRepository.GetById(request.PersonId);
                PersonData data = ToRpcModel(person);
                response.Data = data;
                response.Status.Code = Code.Success;
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(GetById),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = response
				};
                _logger.AddLog(logData);
                return Task.FromResult(response);
            }
            catch (NullReferenceException nrex)
            {
                response.Status.ErrorMessage = "An error occured while loading person data";
                response.Status.Code = Code.DataError;
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(GetById),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = nrex
				};
                _logger.AddErrorLog(logData);
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                response.Status.ErrorMessage = "An error occured while loading person data";
                response.Status.Code = Code.UnknownError;
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(GetById),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = ex
				};
                _logger.AddErrorLog(logData);
                return Task.FromResult(response);
            }
        }

        public override async Task<BaseResponse> AddAsync(PersonData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _peopleRepository.AddAsync(null);

                Person person = new()
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
                BaseResponse response = new()
				{
					Code = Code.Success,
					ErrorMessage = string.Empty,
					DataId = person.Id
				};

                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(AddAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = response
				};

                _logger.AddLog(logData);

                return response;
            }
            catch (NullReferenceException nrex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(AddAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = nrex
				};

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = nrex.Message
                };
            }
            catch (ArgumentException aex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(AddAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = aex
				};

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = aex.Message
                };
            }
            catch (DbUpdateException duex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(AddAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = duex
				};

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving person data"
                };
            }
            catch(Exception ex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(AddAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = ex
				};

                _logger.AddErrorLog(logData);
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

                Person person = new()
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
                BaseResponse response = new()
				{
					Code = Code.Success,
					ErrorMessage = string.Empty,
					DataId = person.Id
				};

                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(UpdateAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = response
				};

                _logger.AddLog(logData);

                return response;
            }
            catch (NullReferenceException nrex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(UpdateAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = nrex
				};

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = nrex.Message
                };
            }
            catch (ArgumentException aex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(UpdateAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = aex
				};

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = aex.Message
                };
            }
            catch (DbUpdateException duex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(UpdateAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = duex
				};

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving person data"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(UpdateAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = ex
				};

                _logger.AddErrorLog(logData);
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

                Contact contact = new()
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
                BaseResponse response = new()
				{
					Code = Code.Success,
					ErrorMessage = string.Empty,
					DataId = contact.Id
				};

                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(AddContactAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = response
				};

                _logger.AddLog(logData);

                return response;
            }
            catch(NullReferenceException nrex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(AddContactAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = nrex
				};

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = nrex.Message
                };
            }
            catch(ArgumentException aex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(AddContactAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = aex
				};

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = aex.Message
                };
            }
            catch(DbUpdateException duex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(AddContactAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = duex
				};

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving contact"
                };
            }
            catch(Exception ex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(AddContactAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = ex
				};

                _logger.AddErrorLog(logData);
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
                PersonPhoto photo = new()
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
                BaseResponse response = new()
				{
					Code = Code.Success,
					ErrorMessage = string.Empty,
					DataId = photo.Id
				};

                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(AddPhotoAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = response
				};

                _logger.AddLog(logData);

                return response;
            }
            catch (NullReferenceException nrex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(AddPhotoAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = nrex
				};

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = nrex.Message
                };
            }
            catch (ArgumentException aex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(AddPhotoAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = aex
				};

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = aex.Message
                };
            }
            catch (DbUpdateException duex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(AddPhotoAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = duex
				};

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving contact"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new()
				{
					CallSide = nameof(PeopleService),
					CallerMethodName = nameof(AddPhotoAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = ex
				};

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.UnknownError,
                    ErrorMessage = ex.Message
                };
            }
        }

        private static PersonData ToRpcModel(Person person)
        {
            PersonData converted = new()
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
