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

    public async Task<Domain.Staff> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        Domain.Staff? staff = await _context.Staff
            .Include(e => e.History)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (staff is null)
            throw new NotFoundException($"Staff with id {id} not found");

        return staff;
    }

    public async Task<IEnumerable<Domain.Staff>> GetByPersonAsync(int personId, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Staff> data = await _context.Staff
            .Include(e => e.History)
            .AsNoTracking()
            .Where(e => e.History!.PersonId == personId)
            .ToListAsync(cancellationToken);
        
        return data;
    }

    public async Task<IEnumerable<Domain.Staff>> GetByManagerAsync(int managerId, CancellationToken cancellationToken)
    {
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

    public async Task<int> CreateAsync(int position, int manager, int person, int employment, DateTime createdOn, int? mentor, CancellationToken cancellationToken)
    {
        Domain.Staff staff = new()
        {
            ManagerId = manager,
            PositionId = position,
            History = new History
            {
                PersonId = person,
                Employment = employment,
                MentorId = mentor,
                CreatedOn = createdOn
            }
        };

        await _context.Staff.AddAsync(staff, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return staff.Id;
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

    public async Task ThrowExceptionIfDateIsWrongAsync(int person, DateTime date, CancellationToken cancellationToken)
    {
        List<Domain.Staff> staff = (await GetByPersonAsync(person, cancellationToken)).ToList(); 
        if (staff.Exists(e => e.History!.CreatedOn.Date > date.Date))
        {
            DateTime minDate = staff.OrderByDescending(e => e.History.CreatedOn)
                .First()
                .History!.CreatedOn.Date
                .AddDays(1);
            throw new AlreadyExistsException($"Employment period for date {date.Date} already exists." +
                                             $"{Environment.NewLine}Minimum start work date is {minDate.Date}");
        }
    }

    private async Task<Domain.Staff> GetStaffAsync(int staffId, CancellationToken cancellationToken)
    {
        Domain.Staff staff = await _context.Staff.FirstOrDefaultAsync(e => e.Id == staffId, cancellationToken) ?? 
            throw new NotFoundException($"Staff with id {staffId} not found");

        return staff;
    }

    private async Task<History> GetStaffHistoryAsync(int staffId, CancellationToken cancellationToken)
    {
        History history = await _context.History.FirstOrDefaultAsync(e => e.StaffId == staffId, cancellationToken) ??
            throw new NotFoundException($"Staff history with id {staffId} not found");

        return history;
    }
}