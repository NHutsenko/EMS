using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace EMS.Core.API.Services
{
    public class HolidaysService : Holidays.HolidaysBase
    {
        private readonly IHolidaysRepository _holidaysRepository;
        private readonly IEMSLogger<HolidaysService> _logger;
        private readonly IDateTimeUtil _dateTimeUtil;

        public HolidaysService(IHolidaysRepository holidaysRepository, IEMSLogger<HolidaysService> logger, IDateTimeUtil dateTimeUtil)
        {
            _holidaysRepository = holidaysRepository;
            _logger = logger;
            _dateTimeUtil = dateTimeUtil;
        }


        public override async Task<BaseResponse> AddAsync(HolidayData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _holidaysRepository.AddAsync(null);
                Holiday holiday = FromRpcModel(request);
                int result = await _holidaysRepository.AddAsync(holiday);
                if (result == 0)
                {
                    throw new Exception("Holiday has not been saved");
                }

                BaseResponse response = new()
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty,
                    DataId = holiday.Id
                };

                LogData logData = new()
				{
					CallSide = nameof(HolidaysService),
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
					CallSide = nameof(HolidaysService),
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
					CallSide = nameof(HolidaysService),
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
					CallSide = nameof(HolidaysService),
					CallerMethodName = nameof(AddAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = duex
				};
                _logger.AddErrorLog(logData);

                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving holiday"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new()
				{
					CallSide = nameof(HolidaysService),
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

        public override async Task<BaseResponse> DeleteAsync(HolidayData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _holidaysRepository.DeleteAsync(null);

                Holiday holiday = FromRpcModel(request);
                int result = await _holidaysRepository.DeleteAsync(holiday);
                if(result == 0)
                {
                    throw new Exception("Holiday has not been deleted");
                }

                BaseResponse response = new()
				{
					Code = Code.Success,
					ErrorMessage = string.Empty,
					DataId = holiday.Id
				};

                LogData logData = new()
				{
					CallSide = nameof(HolidaysService),
					CallerMethodName = nameof(DeleteAsync),
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
					CallSide = nameof(HolidaysService),
					CallerMethodName = nameof(DeleteAsync),
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
            catch (InvalidOperationException ioex)
            {
                LogData logData = new()
				{
					CallSide = nameof(HolidaysService),
					CallerMethodName = nameof(DeleteAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = ioex
				};
                _logger.AddErrorLog(logData);

                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = ioex.Message
                };
            }
            catch (DbUpdateException duex)
            {
                LogData logData = new()
				{
					CallSide = nameof(HolidaysService),
					CallerMethodName = nameof(DeleteAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = duex
				};
                _logger.AddErrorLog(logData);

                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while deleting holiday" 
                };
            }
            catch (Exception ex)
            {
                LogData logData = new()
				{
					CallSide = nameof(HolidaysService),
					CallerMethodName = nameof(DeleteAsync),
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

        public override async Task<BaseResponse> UpdateAsync(HolidayData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _holidaysRepository.UpdateAsync(null);

                Holiday holiday = FromRpcModel(request);
                int result = await _holidaysRepository.UpdateAsync(holiday);
                if (result == 0)
                {
                    throw new Exception("Holiday has not been updated");
                }

                BaseResponse response = new()
				{
					Code = Code.Success,
					ErrorMessage = string.Empty,
					DataId = holiday.Id
				};

                LogData logData = new()
				{
					CallSide = nameof(HolidaysService),
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
					CallSide = nameof(HolidaysService),
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
					CallSide = nameof(HolidaysService),
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
					CallSide = nameof(HolidaysService),
					CallerMethodName = nameof(UpdateAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = duex
				};
                _logger.AddErrorLog(logData);

                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while updating holiday"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new()
				{
					CallSide = nameof(HolidaysService),
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

        public override Task<HolidaysResponse> GetAll(Empty request, ServerCallContext context)
        {
            HolidaysResponse response = new()
			{
				Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty }
			};
            try
            {
                IQueryable<Holiday> holidays = _holidaysRepository.GetAll();

                foreach (Holiday holiday in holidays)
                {
                    response.Data.Add(ToRpcModel(holiday));
                }

                LogData logData = new()
				{
					CallSide = nameof(HolidaysService),
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
					CallSide = nameof(HolidaysService),
					CallerMethodName = nameof(GetAll),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = ex
				};
                _logger.AddErrorLog(logData);
                response.Status.Code = Code.UnknownError;
                response.Status.ErrorMessage = "An error occured while loading holidays data";
            }

            return Task.FromResult(response);
        }

        public override Task<HolidaysResponse> GetByDateRange(DateRangeRequest request, ServerCallContext context)
        {
            HolidaysResponse response = new()
			{
				Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty }
			};
            try
            {
                IQueryable<Holiday> holidays = _holidaysRepository.GetByDateRange(request.From.ToDateTime().Date, request.To.ToDateTime().Date);

                foreach (Holiday holiday in holidays)
                {
                    response.Data.Add(ToRpcModel(holiday));
                }

                LogData logData = new()
				{
					CallSide = nameof(HolidaysService),
					CallerMethodName = nameof(GetByDateRange),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = response
				};
                _logger.AddLog(logData);
            }
            catch (Exception ex)
            {
                LogData logData = new()
				{
					CallSide = nameof(HolidaysService),
					CallerMethodName = nameof(GetByDateRange),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = ex
				};
                _logger.AddErrorLog(logData);
                response.Status.Code = Code.UnknownError;
                response.Status.ErrorMessage = "An error occured while loading holidays data";
            }

            return Task.FromResult(response);
        }

        private static HolidayData ToRpcModel(Holiday holiday)
        {
            return new HolidayData
            {
                Id = holiday.Id,
                CreatedOn = Timestamp.FromDateTime(holiday.CreatedOn),
                Description = holiday.Description,
                HolidayDate = Timestamp.FromDateTime(holiday.HolidayDate),
                ToDoDate = holiday.ToDoDate.HasValue ? Timestamp.FromDateTime(holiday.ToDoDate.Value) : Timestamp.FromDateTime(DateTime.MinValue.ToUniversalTime())
            };
        }

        private static Holiday FromRpcModel(HolidayData holiday)
        {
            return new Holiday
            {
                Id = holiday.Id,
                CreatedOn = holiday.CreatedOn is null ? DateTime.MinValue : holiday.CreatedOn.ToDateTime(),
                Description = holiday.Description,
                HolidayDate = holiday.HolidayDate.ToDateTime(),
                ToDoDate = holiday.ToDoDate.ToDateTime().Date == DateTime.MinValue.Date ? null : holiday.ToDoDate.ToDateTime()
            };
        }
    }
}
