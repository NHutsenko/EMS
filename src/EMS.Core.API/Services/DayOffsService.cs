using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Enums;
using EMS.Core.API.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace EMS.Core.API.Services
{
    public class DayOffsService: DayOffs.DayOffsBase
    {
        private readonly IDayOffRepository _dayOffRepository;
        private readonly IEMSLogger<DayOffsService> _logger;
        private readonly IDateTimeUtil _dateTimeUtil;

        public DayOffsService(IDayOffRepository dayOffRepository, IEMSLogger<DayOffsService> logger, IDateTimeUtil dateTimeUtil)
        {
            _dateTimeUtil = dateTimeUtil;
            _dayOffRepository = dayOffRepository;
            _logger = logger;
        }

        public override async Task<BaseResponse> AddAsync(DayOffData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _dayOffRepository.AddAsync(null);

                DayOff dayOff = FromRpcModel(request);
                int result = await _dayOffRepository.AddAsync(dayOff);
                if(result == 0)
                {
                    throw new Exception("Day off has not been saved");
                }
                BaseResponse response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty,
                    DataId = dayOff.Id
                };

                LogData logData = new LogData
                {
                    CallSide = nameof(DayOffsService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };
                _logger.AddLog(logData);

                return response;
            }
            catch(NullReferenceException nrex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(DayOffsService),
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
            catch(ArgumentException aex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(DayOffsService),
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
            catch(DbUpdateException duex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(DayOffsService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving day off"
                };
            }
            catch(Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(DayOffsService),
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

        public override async Task<BaseResponse> DeleteAsync(DayOffData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _dayOffRepository.DeleteAsync(null);
                DayOff dayOff = FromRpcModel(request);
                int result = await _dayOffRepository.DeleteAsync(dayOff);
                if(result == 0)
                {
                    throw new Exception("Day off has not been deleted");
                }
                BaseResponse response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty,
                    DataId = dayOff.Id
                };

                LogData logData = new LogData
                {
                    CallSide = nameof(DayOffsService),
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
                LogData logData = new LogData
                {
                    CallSide = nameof(DayOffsService),
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
                LogData logData = new LogData
                {
                    CallSide = nameof(DayOffsService),
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
                LogData logData = new LogData
                {
                    CallSide = nameof(DayOffsService),
                    CallerMethodName = nameof(DeleteAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while deleting day off"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(DayOffsService),
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

        public override async Task<BaseResponse> UpdateAsync(DayOffData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _dayOffRepository.UpdateAsync(null);

                DayOff dayOff = FromRpcModel(request);
                int result = await _dayOffRepository.UpdateAsync(dayOff);
                if (result == 0)
                {
                    throw new Exception("Day off has not been saved");
                }
                BaseResponse response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty,
                    DataId = dayOff.Id
                };

                LogData logData = new LogData
                {
                    CallSide = nameof(DayOffsService),
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
                LogData logData = new LogData
                {
                    CallSide = nameof(DayOffsService),
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
                LogData logData = new LogData
                {
                    CallSide = nameof(DayOffsService),
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
                LogData logData = new LogData
                {
                    CallSide = nameof(DayOffsService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while updating day off"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(DayOffsService),
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

        public override Task<DayOffsResponse> GetByPersonId(ByPersonIdRequest request, ServerCallContext context)
        {
            DayOffsResponse response = new DayOffsResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            IQueryable<DayOff> dayOffs = _dayOffRepository.GetByPersonId(request.PersonId);

            foreach(DayOff dayOff in dayOffs)
            {
                response.Data.Add(ToRpcModel(dayOff));
            }

            LogData logData = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(GetByPersonId),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };
            _logger.AddLog(logData);

            return Task.FromResult(response);
        }

        public override Task<DayOffsResponse> GetByPersonIdAndDateRange(ByPersonIdAndDateRangeRequest request, ServerCallContext context)
        {
            DayOffsResponse response = new DayOffsResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            IQueryable<DayOff> dayOffs = _dayOffRepository.GetByDateRangeAndPersonId(request.Range.From.ToDateTime(), request.Range.To.ToDateTime(), request.Person.PersonId);

            foreach (DayOff dayOff in dayOffs)
            {
                response.Data.Add(ToRpcModel(dayOff));
            }

            LogData logData = new LogData
            {
                CallSide = nameof(DayOffsService),
                CallerMethodName = nameof(GetByPersonIdAndDateRange),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };
            _logger.AddLog(logData);

            return Task.FromResult(response);
        }

        private static DayOff FromRpcModel(DayOffData dayOff)
        {
            return new DayOff
            {
                Id = dayOff.Id,
                CreatedOn = dayOff.CreatedOn.ToDateTime(),
                DayOffType = (DayOffType)System.Enum.Parse(typeof(DayOffType), dayOff.DayOffType.ToString(), true),
                Hours = dayOff.Hours,
                IsPaid = dayOff.IsPaid,
                PersonId = dayOff.PersonId
            };
        }

        private static DayOffData ToRpcModel(DayOff dayOff)
        {
            return new DayOffData
            {
                Id = dayOff.Id,
                CreatedOn = Timestamp.FromDateTime(dayOff.CreatedOn),
                DayOffType = (int)dayOff.DayOffType,
                Hours = dayOff.Hours,
                IsPaid = dayOff.IsPaid,
                PersonId = dayOff.PersonId
            };
        }
    }
}
