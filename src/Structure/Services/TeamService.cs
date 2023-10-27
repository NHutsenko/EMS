using EMS.Exceptions;
using EMS.Protos;
using EMS.Structure.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;

namespace EMS.Structure.Services;

public sealed class TeamService : Protos.TeamService.TeamServiceBase
{
    private readonly ITeamRepository _teamRepository;

    public TeamService(ITeamRepository teamRepository)
    {
        _teamRepository = teamRepository;
    }

    public override async Task GetAll(Empty request, IServerStreamWriter<Team> responseStream, ServerCallContext context)
    {
        IEnumerable<Team> data = (await _teamRepository.GetAllAsync(context.CancellationToken))
            .Select(e => new Team
            {
                Id = e.Id,
                Name = e.Name,
                Members =
                {
                    e.Members.Select(m => new Member
                    {
                        Id = m.MemberId,
                        Employment = m.Employment,
                        StartWork = DateTime.SpecifyKind(m.StartWork, DateTimeKind.Utc).ToTimestamp(),
                        EndWork = m.EndWork.HasValue ? DateTime.SpecifyKind(m.EndWork.Value, DateTimeKind.Utc).ToTimestamp() : null
                    })
                }
            });

        await responseStream.WriteAllAsync(data);
    }

    public override async Task<Int32Value> Create(StringValue request, ServerCallContext context)
    {
        int id = await _teamRepository.CreateAsync(request.Value, context.CancellationToken);

        return new Int32Value
        {
            Value = id
        };
    }

    public override async Task<Empty> AddMember(NewMemberRequest request, ServerCallContext context)
    {
        await _teamRepository.AddMemberAsync(request.TeamId, 
            request.MemberId, 
            request.Employment,
            request.StartWork.ToDateTime(), 
            context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> UpdateName(NewNameRequest request, ServerCallContext context)
    {
        await _teamRepository.UpdateNameAsync(request.TeamId, request.Name, context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> UpdateEmployment(EmploymentRequest request, ServerCallContext context)
    {
        await _teamRepository.UpdateEmploymentAsync(request.TeamId, request.MemberId, request.Employment, context.CancellationToken);
        
        return new Empty();
    }

    public override async Task<Empty> SetEndWork(EndWorkRequest request, ServerCallContext context)
    {
        await _teamRepository.SetEndWorkAsync(request.TeamId, 
            request.MemberId, 
            request.Date.ToDateTime(), 
            context.CancellationToken);

        return new Empty();
    }

    
}