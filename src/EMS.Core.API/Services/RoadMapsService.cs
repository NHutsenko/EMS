using System;
using System.Collections.Generic;
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
    public class RoadMapsService : RoadMaps.RoadMapsBase
    {
        private readonly IRoadMapRepository _roadMapRepository;
        private readonly IEMSLogger<RoadMapsService> _logger;
        private readonly IDateTimeUtil _dateTimeUtil;

        public RoadMapsService(IRoadMapRepository roadMapRepository, IEMSLogger<RoadMapsService> logger, IDateTimeUtil dateTimeUtil)
        {
            _roadMapRepository = roadMapRepository;
            _logger = logger;
            _dateTimeUtil = dateTimeUtil;
        }

        public override async Task<BaseResponse> AddAsync(RoadMapData request, ServerCallContext context)
        {
            try
            {
                RoadMap roadMap = FromRpcModel(request);
                int result = await _roadMapRepository.AddAsync(roadMap);
                if(result == 0)
                {
                    throw new Exception("Road map was not saved");
                }
                BaseResponse response = new()
                {
                    Code = Code.Success,
                    DataId = roadMap.Id,
                    ErrorMessage = string.Empty
                };
                LogData log = new()
                {
                    CallSide = nameof(RoadMapsService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };
                _logger.AddLog(log);
                return response;
            }
            catch(ArgumentException aex)
            {
                LogData log = new()
                {
                    CallSide = nameof(RoadMapsService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = aex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = aex.Message
                };
            }
            catch(InvalidOperationException ioex)
            {
                LogData log = new()
                {
                    CallSide = nameof(RoadMapsService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ioex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = ioex.Message
                };
            }
            catch (DbUpdateException duex)
            {
                LogData log = new()
                {
                    CallSide = nameof(RoadMapsService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving road map"
                };
            }
            catch(Exception ex)
            {
                LogData log = new()
                {
                    CallSide = nameof(RoadMapsService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    Code = Code.UnknownError,
                    ErrorMessage = ex.Message
                };
            }
        }

        public override async Task<BaseResponse> DeleteAsync(RoadMapData request, ServerCallContext context)
        {
            try
            {
                RoadMap roadMap = FromRpcModel(request);
                int result = await _roadMapRepository.DeleteAsync(roadMap);
                if (result == 0)
                {
                    throw new Exception("Road map was not deleted");
                }
                BaseResponse response = new()
                {
                    Code = Code.Success,
                    DataId = roadMap.Id,
                    ErrorMessage = string.Empty
                };
                LogData log = new()
                {
                    CallSide = nameof(RoadMapsService),
                    CallerMethodName = nameof(DeleteAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };
                _logger.AddLog(log);
                return response;
            }
            catch (DbUpdateException duex)
            {
                LogData log = new()
                {
                    CallSide = nameof(RoadMapsService),
                    CallerMethodName = nameof(DeleteAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while deleting road map"
                };
            }
            catch (Exception ex)
            {
                LogData log = new()
                {
                    CallSide = nameof(RoadMapsService),
                    CallerMethodName = nameof(DeleteAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    Code = Code.UnknownError,
                    ErrorMessage = ex.Message
                };
            }
        }

        public override Task<RoadMapResponse> GetByStaffId(ByStaffRequest request, ServerCallContext context)
        {
            try
            {
                RoadMap roadMap = _roadMapRepository.GetByStaffId(request.StaffId);
                RoadMapResponse response = new()
                {
                    Status = new BaseResponse
                    {
                        Code = Code.Success,
                        ErrorMessage = string.Empty
                    },
                    Data = ToRpcModel(roadMap)
                };
                LogData log = new()
                {
                    CallSide = nameof(RoadMapsService),
                    CallerMethodName = nameof(GetByStaffId),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };
                _logger.AddLog(log);
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                LogData log = new()
                {
                    CallSide = nameof(RoadMapsService),
                    CallerMethodName = nameof(GetByStaffId),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(log);
                return Task.FromResult(new RoadMapResponse
                {
                    Status = new BaseResponse
                    {
                        Code = Code.UnknownError,
                        ErrorMessage = "An error occured while loading Road Map"
                    }
                });
            }
        }

        public override async Task<BaseResponse> UpdateAsync(RoadMapData request, ServerCallContext context)
        {
            try
            {
                RoadMap roadMap = FromRpcModel(request);
                int result = await _roadMapRepository.UpdateAsync(roadMap);
                if (result == 0)
                {
                    throw new Exception("Road map was not updated");
                }
                BaseResponse response = new()
                {
                    Code = Code.Success,
                    DataId = roadMap.Id,
                    ErrorMessage = string.Empty
                };
                LogData log = new()
                {
                    CallSide = nameof(RoadMapsService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };
                _logger.AddLog(log);
                return response;
            }
            catch (ArgumentException aex)
            {
                LogData log = new()
                {
                    CallSide = nameof(RoadMapsService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = aex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = aex.Message
                };
            }
            catch (DbUpdateException duex)
            {
                LogData log = new()
                {
                    CallSide = nameof(RoadMapsService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while updating road map"
                };
            }
            catch (Exception ex)
            {
                LogData log = new()
                {
                    CallSide = nameof(RoadMapsService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    Code = Code.UnknownError,
                    ErrorMessage = ex.Message
                };
            }
        }

        private static RoadMap FromRpcModel(RoadMapData roadMap)
        {
            return new RoadMap
            {
                Id = roadMap.Id,
                CreatedOn = roadMap.CreatedOn == null ? DateTime.MinValue : roadMap.CreatedOn.ToDateTime(),
                StaffId = roadMap.StaffId,
                Status = (RoadMapStatus)System.Enum.Parse(typeof(RoadMapStatus), roadMap.Status.ToString(), true),
                Tasks = roadMap.Tasks
            };
        }

        private static RoadMapData ToRpcModel(RoadMap roadMap)
        {
            return new RoadMapData
            {
                Id = roadMap.Id,
                CreatedOn = Timestamp.FromDateTime(roadMap.CreatedOn),
                Tasks = roadMap.Tasks,
                Status = (int)roadMap.Status,
                StaffId = roadMap.StaffId
            };
        }
    }
}
