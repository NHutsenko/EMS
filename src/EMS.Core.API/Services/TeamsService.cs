using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common.Protos;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMS.Core.API.Services
{
    public class TeamsService : Teams.TeamsBase
    {
        private readonly ILogger<TeamsService> _logger;
        private readonly ITeamsRepository _teamsRepository;

        public TeamsService(ILogger<TeamsService> logger, ITeamsRepository teamsRepository)
        {
            _logger = logger;
            _teamsRepository = teamsRepository;
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
            throw new NotImplementedException();
        }

        public override async Task<BaseResponse> UpdateAsync(TeamData request, ServerCallContext context)
        {
            try
            {
                throw new NotImplementedException();
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
