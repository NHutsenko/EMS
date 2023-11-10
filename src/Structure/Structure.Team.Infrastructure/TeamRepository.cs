using EMS.Exceptions;
using EMS.Structure.Team.Application.Interfaces;
using EMS.Structure.Team.Domain;
using EMS.Structure.Team.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EMS.Structure.Team.Infrastructure;

public sealed class TeamRepository: ITeamRepository
{
    private readonly TeamContext _context;

    public TeamRepository(TeamContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(string name, CancellationToken cancellationToken)
    {
        if (await _context.Teams.AnyAsync(e => e.Name == name, cancellationToken))
            throw new AlreadyExistsException($"Team with name {name} already exists");

        Domain.Team team = new()
        {
            Name = name
        };

        await _context.AddAsync(team, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return team.Id;
    }

    public async Task<IEnumerable<Domain.Team>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Teams
            .Include(e => e.Members)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddMemberAsync(int teamId, int memberId, int employment, DateTime startWork, CancellationToken cancellationToken)
    {
        if (await _context.Members.AnyAsync(e => e.MemberId == memberId && e.TeamId == teamId, cancellationToken: cancellationToken))
            throw new AlreadyExistsException($"Member already in team");
        
        await CheckEmploymentAsync(memberId, teamId, employment, cancellationToken);

        Member member = new()
        {
            Employment = employment,
            StartWork = startWork,
            MemberId = memberId,
            TeamId = teamId
        };

        await _context.AddAsync(member,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateNameAsync(int id, string name, CancellationToken cancellationToken)
    {
        Domain.Team? team = await _context.Teams.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (team is null)
            throw new NotFoundException($"Team not found");

        _context.Entry(team).Property(e => e.Name).CurrentValue = name;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateEmploymentAsync(int teamId, int memberId, int employment, CancellationToken cancellationToken)
    {
        Member member = await GetMemberAsync(memberId, teamId, cancellationToken);

        await CheckEmploymentAsync(memberId, teamId, employment, cancellationToken);

        _context.Entry(member).Property(e => e.Employment).CurrentValue = employment;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SetEndWorkAsync(int teamId, int memberId, DateTime date, CancellationToken cancellationToken)
    {
        Member member = await GetMemberAsync(memberId, teamId, cancellationToken);

        _context.Entry(member).Property(e => e.EndWork).CurrentValue = date;
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    private async Task CheckEmploymentAsync(int memberId, int teamId, int employment, CancellationToken cancellationToken)
    {
        int employmentData = await _context.Members
            .Where(e => e.MemberId == memberId && e.TeamId != teamId)
            .SumAsync(e => e.Employment, cancellationToken);
        
        if (employmentData + employment > 100)
            throw new BadRequestException($"Summary employment cannot be greater than 100");
    }

    private async Task<Member> GetMemberAsync(int memberId, int teamId, CancellationToken cancellationToken)
    {
        Member? member = await _context.Members
            .FirstOrDefaultAsync(e => e.MemberId == memberId && e.TeamId == teamId, cancellationToken);

        if (member is null)
            throw new NotFoundException($"Not a member of selected team");

        return member;
    }
}