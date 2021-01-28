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
    public class PositionsService : Positions.PositionsBase
    {
        private readonly IEMSLogger<PositionsService> _logger;
        private readonly IDateTimeUtil _dateTimeUtil;
        private readonly IPositionsRepository _positionsRepository;

        public PositionsService(IPositionsRepository positionsRepository, IEMSLogger<PositionsService> logger, IDateTimeUtil dateTimeUtil)
        {
            _positionsRepository = positionsRepository;
            _logger = logger;
            _dateTimeUtil = dateTimeUtil;
        }

        public override async Task<BaseResponse> AddAsync(PositionData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _positionsRepository.AddAsync(null);

                Position position = FromRpcModel(request);
                int result = await _positionsRepository.AddAsync(position);
                if (result == 0)
                {
                    throw new Exception("Position has not been saved");
                }

                BaseResponse response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty,
                    DataId = position.Id
                };

                LogData logData = new LogData
                {
                    CallSide = nameof(PositionsService),
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
                LogData logData = new LogData
                {
                    CallSide = nameof(PositionsService),
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
                LogData logData = new LogData
                {
                    CallSide = nameof(PositionsService),
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
                LogData logData = new LogData
                {
                    CallSide = nameof(PositionsService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };
                _logger.AddErrorLog(logData);

                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving position"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(PositionsService),
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

        public override async Task<BaseResponse> DeleteAsync(PositionData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _positionsRepository.DeleteAsync(null);

                Position position = FromRpcModel(request);
                int result = await _positionsRepository.DeleteAsync(position);
                if (result == 0)
                {
                    throw new Exception("Position has not been deleted");
                }

                BaseResponse response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty,
                    DataId = position.Id
                };

                LogData logData = new LogData
                {
                    CallSide = nameof(PositionsService),
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
                    CallSide = nameof(PositionsService),
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
                    CallSide = nameof(PositionsService),
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
                    CallSide = nameof(PositionsService),
                    CallerMethodName = nameof(DeleteAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };
                _logger.AddErrorLog(logData);

                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while deleting position"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(PositionsService),
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

        public override async Task<BaseResponse> UpdateAsync(PositionData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _positionsRepository.UpdateAsync(null);

                Position position = FromRpcModel(request);
                int result = await _positionsRepository.UpdateAsync(position);
                if (result == 0)
                {
                    throw new Exception("Position has not been updated");
                }

                BaseResponse response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty,
                    DataId = position.Id
                };

                LogData logData = new LogData
                {
                    CallSide = nameof(PositionsService),
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
                    CallSide = nameof(PositionsService),
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
                    CallSide = nameof(PositionsService),
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
                    CallSide = nameof(PositionsService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };
                _logger.AddErrorLog(logData);

                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while updating position"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(PositionsService),
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

        public override Task<PositionsResponse> GetAll(Empty request, ServerCallContext context)
        {
            IQueryable<Position> positions = _positionsRepository.GetAll();
            PositionsResponse response = new PositionsResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            foreach (Position position in positions)
            {
                response.Data.Add(ToRpcModel(position));
            }

            LogData logData = new LogData
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                CallerMethodName = nameof(GetAll),
                CallSide = nameof(PositionsService),
                Request = request,
                Response = response
            };
            _logger.AddLog(logData);

            return Task.FromResult(response);
        }

        public override Task<PositionResponse> GetById(PositionRequest request, ServerCallContext context)
        {
            Position position = _positionsRepository.Get(request.PositionId);
            PositionResponse response = new PositionResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            if (position is null)
            {
                response.Status.Code = Code.DataError;
                response.Status.ErrorMessage = "Requested position not found";
            }
            else
            {
                response.Data = ToRpcModel(position);
            }

            LogData logData = new LogData
            {
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                CallerMethodName = nameof(GetById),
                CallSide = nameof(PositionsService),
                Request = request,
                Response = response
            };
            _logger.AddLog(logData);

            return Task.FromResult(response);
        }

        private static Position FromRpcModel(PositionData position)
        {
            return new Position
            {
                Id = position.Id,
                CreatedOn = position.CreatedOn.ToDateTime(),
                Name = position.Name,
                HourRate = position.HourRate,
                TeamId = position.TeamId
            };
        }

        private static PositionData ToRpcModel(Position position)
        {
            return new PositionData
            {
                Id = position.Id,
                CreatedOn = Timestamp.FromDateTime(position.CreatedOn),
                Name = position.Name,
                HourRate = position.HourRate,
                TeamId = position.TeamId
            };
        }
    }
}
