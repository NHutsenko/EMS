using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common.Models.BaseModel;
using EMS.Common.Protos;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LoggerExtensions;
using Newtonsoft.Json;
using EMS.Common.Utils.DateTimeUtil;

namespace EMS.Core.API.Services
{
    public class TeamsService : Teams.TeamsBase
    {
        private readonly ITeamsRepository _teamsRepository;
        private readonly IDateTimeUtil _dateTimeUtil;

        public TeamsService(ITeamsRepository teamsRepository, IDateTimeUtil dateTimeUtil)
        {
            _teamsRepository = teamsRepository;
            _dateTimeUtil = dateTimeUtil;
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
                if(result == 0)
                {
                    throw new Exception("Team has not been saved");
                }
                BaseResponse response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                };

                RequestResponseObject rrObject = new RequestResponseObject
                {
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };

                return response;
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
                    ErrorMessage = "An error occured while saving team"
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
                if(result == 0)
                {
                    throw new Exception("Team has not been deleted");
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
            catch(InvalidOperationException oex)
            {
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = oex.Message
                };
            }
            catch(DbUpdateException duex)
            {
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while deleting team"
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
                    ErrorMessage = "An error occured while saving team"
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

        public override Task<TeamsResponse> GetAll(Empty request, ServerCallContext context)
        {
            IQueryable<Team> teams = _teamsRepository.GetAll();

            TeamsResponse response = new TeamsResponse();
            response.Response = new BaseResponse
            {
                Code = Code.Success,
                ErrorMessage = string.Empty
            };

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
            return Task.FromResult(response);
        }

        public override Task<TeamResponse> GetById(TeamRequest request, ServerCallContext context)
        {
            Team team = _teamsRepository.Get(request.Id);
            TeamResponse response = new TeamResponse
            {
                Response = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            if(team is null)
            {
                response.Response.Code = Code.DataError;
                response.Response.ErrorMessage = "Requested team not found";
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

            return Task.FromResult(response);
        }
    }
}
