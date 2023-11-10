using EMS.Exceptions;
using EMS.Staff.Application.Interfaces;
using EMS.Staff.Domain;
using EMS.Staff.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EMS.Staff.Infrastructure;

public sealed class StaffRepository: IStaffRepository
{
    private readonly StaffContext _context;

    public StaffRepository(StaffContext context)
    {
        _context = context;
    }

    public async Task<Domain.Staff> GetByHistoryIdAsync(int historyId, CancellationToken cancellationToken)
    {
        Domain.Staff? staff = await _context.Staff
            .Include(e => e.History)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.History!.Id == historyId, cancellationToken);
        if (staff is null)
            throw new NotFoundException($"Staff with history id {historyId} not found");

        return staff;
    }

    public async Task<IEnumerable<Domain.Staff>> GetByPersonAsync(int personId, CancellationToken cancellationToken)
    {
        if (await _context.History.AnyAsync(e => e.PersonId == personId, cancellationToken) is false)
            throw new NotFoundException($"Staff for person with id {personId} not found");

        IEnumerable<Domain.Staff> data = await _context.Staff
            .Include(e => e.History)
            .AsNoTracking()
            .Where(e => e.History!.PersonId == personId)
            .ToListAsync(cancellationToken);
        
        return data;
    }

    public async Task<IEnumerable<Domain.Staff>> GetByManagerAsync(int managerId, CancellationToken cancellationToken)
    {
        if (await _context.Staff.AnyAsync(e => e.ManagerId == managerId, cancellationToken) is false)
            throw new NotFoundException($"Staff for manager with id {managerId} not found");

        IEnumerable<Domain.Staff> data = await _context.Staff
            .Include(e => e.History)
            .AsNoTracking()
            .Where(e => e.ManagerId == managerId)
            .ToListAsync(cancellationToken);
        return data;
    }

    public async Task<IEnumerable<Domain.Staff>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Staff
            .Include(e => e.History)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CreateAsync(int position, int manager, CancellationToken cancellationToken)
    {
        Domain.Staff staff = new()
        {
            ManagerId = manager,
            PositionId = position
        };

        await _context.Staff.AddAsync(staff, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return staff.Id;
    }

    public async Task CreateHistoryAsync(int staffId, int person, int? mentor, int employment, DateTime createdOn, CancellationToken cancellationToken)
    {
        if (await _context.Staff.AnyAsync(e => e.Id == staffId, cancellationToken: cancellationToken) is false)
            throw new NotFoundException($"Staff with id {staffId} not found");

        History history = new()
        {
            StaffId = staffId,
            PersonId = person,
            MentorId = mentor,
            Employment = employment,
            CreatedOn = createdOn
        };

        await _context.History.AddAsync(history, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SetManagerAsync(int staffId, int manager, CancellationToken cancellationToken)
    {
        Domain.Staff staff = await GetStaffAsync(staffId, cancellationToken);

        _context.Entry(staff).Property(e => e.ManagerId).CurrentValue = manager;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SetPositionAsync(int staffId, int position, CancellationToken cancellationToken)
    {
        Domain.Staff staff = await GetStaffAsync(staffId, cancellationToken);

        _context.Entry(staff).Property(e => e.PositionId).CurrentValue = position;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SetDateAsync(int staffId, DateTime createdOn, CancellationToken cancellationToken)
    {
        History history = await GetStaffHistoryAsync(staffId, cancellationToken);

        _context.Entry(history).Property(e => e.CreatedOn).CurrentValue = createdOn;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SetEmploymentAsync(int staffId, int employment, CancellationToken cancellationToken)
    {
        History history = await GetStaffHistoryAsync(staffId, cancellationToken);

        _context.Entry(history).Property(e => e.Employment).CurrentValue = employment;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SetMentorAsync(int staffId, int? mentor, CancellationToken cancellationToken)
    {
        History history = await GetStaffHistoryAsync(staffId, cancellationToken);

        _context.Entry(history).Property(e => e.MentorId).CurrentValue = mentor;
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    private async Task<Domain.Staff> GetStaffAsync(int staffId, CancellationToken cancellationToken)
    {
        Domain.Staff? staff = await _context.Staff.FirstOrDefaultAsync(e => e.Id == staffId, cancellationToken);
        if (staff is null)
            throw new NotFoundException($"Staff with id {staffId} not found");

        return staff;
    }

    private async Task<History> GetStaffHistoryAsync(int staffId, CancellationToken cancellationToken)
    {
        History? history = await _context.History.FirstOrDefaultAsync(e => e.StaffId == staffId, cancellationToken);
        if (history is null)
            throw new NotFoundException($"Staff history with id {staffId} not found");

        return history;
    }
}