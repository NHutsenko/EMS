using EMS.Protos;
using EMS.Staff.Context;
using Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace EMS.Staff.Services;

public sealed class TeamService : Protos.TeamService.TeamServiceBase
{
    private readonly StaffContext _dbContext;

    public TeamService(StaffContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<TeamsReply> GetAll(Empty request, ServerCallContext context)
    {
        IEnumerable<Team> data = (await _dbContext.Teams
                .Include(e => e.Members)
                .AsNoTracking()
                .ToListAsync(context.CancellationToken))
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

        return new TeamsReply
        {
            Data = { data }
        };
    }

    public override async Task<Int32Value> Create(StringValue request, ServerCallContext context)
    {
        if (await _dbContext.Teams.AnyAsync(e => e.Name == request.Value, context.CancellationToken))
        {
            throw new AlreadyExistsException($"Team with name {request.Value} already exists");
        }

        Models.Team team = new()
        {
            Name = request.Value
        };

        await _dbContext.AddAsync(team, context.CancellationToken);
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new Int32Value
        {
            Value = team.Id
        };
    }

    public override async Task<Empty> AddMember(NewMemberRequest request, ServerCallContext context)
    {
        if (await _dbContext.Members.AnyAsync(e => e.MemberId == request.MemberId && e.TeamId == request.TeamId))
        {
            throw new AlreadyExistsException($"Member already in team");
        }
        
        await CheckEmploymentAsync(request.MemberId, request.TeamId, request.Employment, context.CancellationToken);

        Models.Member member = new()
        {
            Employment = request.Employment,
            StartWork = request.StartWork.ToDateTime(),
            MemberId = request.MemberId,
            TeamId = request.TeamId
        };

        await _dbContext.AddAsync(member, context.CancellationToken);
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> UpdateName(NewNameRequest request, ServerCallContext context)
    {
        Models.Team? team = await _dbContext.Teams.FirstOrDefaultAsync(e => e.Id == request.TeamId, context.CancellationToken);
        if (team is null)
        {
            throw new NotFoundException($"Team not found");
        }

        _dbContext.Entry(team).Property(e => e.Name).CurrentValue = request.Name;
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> UpdateEmployment(EmploymentRequest request, ServerCallContext context)
    {
        Models.Member member = await GetMemberAsync(request.MemberId, request.TeamId, context.CancellationToken);

        await CheckEmploymentAsync(request.MemberId, request.TeamId, request.Employment, context.CancellationToken);

        _dbContext.Entry(member).Property(e => e.Employment).CurrentValue = request.Employment;
        await _dbContext.SaveChangesAsync(context.CancellationToken);
        return new Empty();
    }

    public override async Task<Empty> SetEndWork(EndWorkRequest request, ServerCallContext context)
    {
        Models.Member member = await GetMemberAsync(request.MemberId, request.TeamId, context.CancellationToken);

        _dbContext.Entry(member).Property(e => e.EndWork).CurrentValue = request.Date.ToDateTime();
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new Empty();
    }

    private async Task CheckEmploymentAsync(int memberId, int teamId, int employment, CancellationToken cancellationToken)
    {
        int employmentData = await _dbContext.Members
            .Where(e => e.MemberId == memberId && e.TeamId != teamId)
            .SumAsync(e => e.Employment, cancellationToken);
        if (employmentData + employment > 100)
        {
            throw new BadRequestException($"Summary employment cannot be greater than 100");
        }
    }

    private async Task<Models.Member> GetMemberAsync(int memberId, int teamId, CancellationToken cancellationToken)
    {
        Models.Member? member = await _dbContext.Members
            .FirstOrDefaultAsync(e => e.MemberId == memberId && e.TeamId == teamId, cancellationToken);

        if (member is null)
        {
            throw new NotFoundException($"Not a member of selected team");
        }

        return member;
    }
}