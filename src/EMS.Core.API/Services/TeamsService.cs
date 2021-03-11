using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common.Protos;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;

namespace EMS.Core.API.Services
{
    public class TeamsService : Teams.TeamsBase
    {
        private readonly ITeamsRepository _teamsRepository;
        private readonly IDateTimeUtil _dateTimeUtil;
        private readonly IEMSLogger<TeamsService> _logger;

        public TeamsService(ITeamsRepository teamsRepository, IDateTimeUtil dateTimeUtil, IEMSLogger<TeamsService> logger)
        {
            _teamsRepository = teamsRepository;
            _dateTimeUtil = dateTimeUtil;
            _logger = logger;
        }

        public override async Task<BaseResponse> AddAsync(TeamData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _teamsRepository.AddAsync(null);
                Team team = new Team
                {
                    Name = request.Name,
                    Description = request.Description,
                };
                int result = await _teamsRepository.AddAsync(team);
                if (result == 0)
                {
                    throw new Exception("Team has not been saved");
                }
                BaseResponse response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty,
                    DataId = team.Id
                };

                LogData logData = new LogData
                {
                    CallSide = nameof(TeamsService),
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
                    CallSide = nameof(TeamsService),
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
                    CallSide = nameof(TeamsService),
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
                    CallSide = nameof(TeamsService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving team"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(TeamsService),
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

        public override async Task<BaseResponse> DeleteAsync(TeamData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _teamsRepository.DeleteAsync(null);

                Team team = new Team
                {
                    Id = request.Id,
                    CreatedOn = request.CreatedOn.ToDateTime().ToLocalTime(),
                    Name = request.Name,
                    Description = request.Description
                };
                int result = await _teamsRepository.DeleteAsync(team);
                if (result == 0)
                {
                    throw new Exception("Team has not been deleted");
                }
                BaseResponse response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty,
                    DataId = team.Id
                };
                LogData logData = new LogData
                {
                    CallSide = nameof(TeamsService),
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
                    CallSide = nameof(TeamsService),
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
            catch (InvalidOperationException oex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(TeamsService),
                    CallerMethodName = nameof(DeleteAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = oex
                };

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = oex.Message
                };
            }
            catch (DbUpdateException duex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(TeamsService),
                    CallerMethodName = nameof(DeleteAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };

                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while deleting team"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(TeamsService),
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

        public override async Task<BaseResponse> UpdateAsync(TeamData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _teamsRepository.UpdateAsync(null);

                Team team = new Team
                {
                    Id = request.Id,
                    CreatedOn = request.CreatedOn.ToDateTime().ToLocalTime(),
                    Name = request.Name,
                    Description = request.Description
                };

                int result = await _teamsRepository.UpdateAsync(team);
                if (result == 0)
                {
                    throw new Exception("Team has not been updated");
                }
                BaseResponse response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty,
                    DataId = team.Id
                };
                LogData logData = new LogData
                {
                    CallSide = nameof(TeamsService),
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
                    CallSide = nameof(TeamsService),
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
                    CallSide = nameof(TeamsService),
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
                    CallSide = nameof(TeamsService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving team"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(TeamsService),
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

        public override Task<TeamsResponse> GetAll(Empty request, ServerCallContext context)
        {
            TeamsResponse response = new TeamsResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            try
            {
                IQueryable<Team> teams = _teamsRepository.GetAll();


                foreach (Team team in teams)
                {
                    TeamData data = new TeamData
                    {
                        Id = team.Id,
                        CreatedOn = Timestamp.FromDateTime(team.CreatedOn.ToUniversalTime()),
                        Name = team.Name,
                        Description = team.Description
                    };
                    response.Data.Add(data);
                }
                LogData logData = new LogData
                {
                    CallSide = nameof(TeamsService),
                    CallerMethodName = nameof(GetAll),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };
                _logger.AddLog(logData);
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(TeamsService),
                    CallerMethodName = nameof(GetAll),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                response.Status.Code = Code.UnknownError;
                response.Status.ErrorMessage = "An error occured while loading teams data";
            }
            return Task.FromResult(response);
        }

        public override Task<TeamResponse> GetById(TeamRequest request, ServerCallContext context)
        {
            TeamResponse response = new TeamResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            try
            {
                Team team = _teamsRepository.Get(request.Id);

                if (team is null)
                {
                    response.Status.Code = Code.DataError;
                    response.Status.ErrorMessage = "Requested team not found";
                }
                else
                {
                    response.Data = new TeamData
                    {
                        Id = team.Id,
                        CreatedOn = Timestamp.FromDateTime(team.CreatedOn.ToUniversalTime()),
                        Name = team.Name,
                        Description = team.Description
                    };
                }

                LogData logData = new LogData
                {
                    CallSide = nameof(TeamsService),
                    CallerMethodName = nameof(GetById),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };
                _logger.AddLog(logData);
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(TeamsService),
                    CallerMethodName = nameof(GetById),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                response.Status.Code = Code.UnknownError;
                response.Status.ErrorMessage = "An error occured while loading team data";
            }
            return Task.FromResult(response);
        }
    }
}
