using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common.Protos;
using EMS.Core.API.DAL.Repositories.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace EMS.Core.API.Services
{
    public class TeamsService: Teams.TeamsBase
    {
        private readonly ILogger<TeamsService> _logger;
        private readonly ITeamsRepository _teamsRepository;

        public TeamsService(ILogger<TeamsService> logger, ITeamsRepository teamsRepository)
        {
            _logger = logger;
            _teamsRepository = teamsRepository;
        }

        public override async Task<BaseResponse> AddAsync(Team request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override async Task<BaseResponse> DeleteAsync(Team request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override async Task<BaseResponse> UpdateAsync(Team request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<TeamsResponse> GetAll(Empty request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<TeamResponse> GetById(TeamRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }
}
