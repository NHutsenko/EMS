using EMS.Extensions;
using EMS.Protos;
using EMS.Staff.Context;
using EMS.Staff.Models;
using Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace EMS.Staff.Services;

public sealed class StaffService : Protos.StaffService.StaffServiceBase
{
    private readonly StaffContext _dbContext;

    public StaffService(StaffContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task GetByPerson(Int32Value request, IServerStreamWriter<Protos.Staff> responseStream, ServerCallContext context)
    {
        if (await _dbContext.History.AnyAsync(e => e.PersonId == request.Value, context.CancellationToken) is false)
        {
            throw new NotFoundException($"Staff for person with id {request.Value} not found");
        }

        IEnumerable<Protos.Staff> data = (await _dbContext.Staff
                .Include(e => e.History)
                .AsNoTracking()
                .Where(e => e.History.PersonId == request.Value)
                .ToListAsync(context.CancellationToken))
            .Select(e => new Protos.Staff
            {
                Id = e.Id,
                Position = e.PositionId,
                Manager = e.ManagerId,
                History = e.History == null
                    ? null
                    : new StaffHistory
                    {
                        Person = e.History.PersonId,
                        Mentor = e.History.MentorId,
                        Employment = e.History.Employment,
                        CreatedOn = DateTime.SpecifyKind(e.History.CreatedOn, DateTimeKind.Utc).ToTimestamp()
                    }
            });
        await responseStream.WriteResponseAsync(data, context.CancellationToken);
    }

    public override async Task GetByManager(Int32Value request, IServerStreamWriter<Protos.Staff> responseStream, ServerCallContext context)
    {
        if (await _dbContext.Staff.AnyAsync(e => e.ManagerId == request.Value, context.CancellationToken) is false)
        {
            throw new NotFoundException($"Staff for manager with id {request.Value} not found");
        }

        IEnumerable<Protos.Staff> data = (await _dbContext.Staff
                .Include(e => e.History)
                .AsNoTracking()
                .Where(e => e.ManagerId == request.Value)
                .ToListAsync(context.CancellationToken))
            .Select(e => new Protos.Staff
            {
                Id = e.Id,
                Position = e.PositionId,
                Manager = e.ManagerId,
                History = e.History == null
                    ? null
                    : new StaffHistory
                    {
                        Person = e.History.PersonId,
                        Mentor = e.History.MentorId,
                        Employment = e.History.Employment,
                        CreatedOn = DateTime.SpecifyKind(e.History.CreatedOn, DateTimeKind.Utc).ToTimestamp()
                    }
            });
        await responseStream.WriteResponseAsync(data, context.CancellationToken);
    }

    public override async Task GetAll(Empty request, IServerStreamWriter<Protos.Staff> responseStream, ServerCallContext context)
    {
        IEnumerable<Protos.Staff> data = (await _dbContext.Staff
                .Include(e => e.History)
                .AsNoTracking()
                .ToListAsync(context.CancellationToken))
            .Select(e => new Protos.Staff
            {
                Id = e.Id,
                Position = e.PositionId,
                Manager = e.ManagerId,
                History = e.History == null
                    ? null
                    : new StaffHistory
                    {
                        Person = e.History.PersonId,
                        Mentor = e.History.MentorId,
                        Employment = e.History.Employment,
                        CreatedOn = DateTime.SpecifyKind(e.History.CreatedOn, DateTimeKind.Utc).ToTimestamp()
                    }
            });

        await responseStream.WriteResponseAsync(data, context.CancellationToken);
    }

    public override async Task<Int32Value> Create(NewStaff request, ServerCallContext context)
    {
        Models.Staff staff = new()
        {
            ManagerId = request.Manager,
            PositionId = request.Position
        };

        await _dbContext.Staff.AddAsync(staff, context.CancellationToken);
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new Int32Value
        {
            Value = staff.Id
        };
    }

    public override async Task<Empty> CreateHistory(NewHistory request, ServerCallContext context)
    {
        if (await _dbContext.Staff.AnyAsync(e => e.Id == request.StaffId) is false)
        {
            throw new NotFoundException($"Staff with id {request.StaffId} not found");
        }

        History history = new()
        {
            StaffId = request.StaffId,
            PersonId = request.Data.Person,
            MentorId = request.Data.Mentor,
            Employment = request.Data.Employment,
            CreatedOn = request.Data.CreatedOn.ToDateTime()
        };

        await _dbContext.History.AddAsync(history, context.CancellationToken);
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> SetManager(NewManager request, ServerCallContext context)
    {
        Models.Staff staff = await GetStaffAsync(request.StaffId, context.CancellationToken);

        _dbContext.Entry(staff).Property(e => e.ManagerId).CurrentValue = request.Manager;
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> SetPosition(NewPosition request, ServerCallContext context)
    {
        Models.Staff staff = await GetStaffAsync(request.StaffId, context.CancellationToken);

        _dbContext.Entry(staff).Property(e => e.PositionId).CurrentValue = request.Position;
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> SetDate(NewDate request, ServerCallContext context)
    {
        History history = await GetStaffHistoryAsync(request.StaffId, context.CancellationToken);

        _dbContext.Entry(history).Property(e => e.CreatedOn).CurrentValue = request.Date.ToDateTime();
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> SetEmployment(NewEmployment request, ServerCallContext context)
    {
        History history = await GetStaffHistoryAsync(request.StaffId, context.CancellationToken);

        _dbContext.Entry(history).Property(e => e.Employment).CurrentValue = request.Employment;
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> SetMentor(NewMentor request, ServerCallContext context)
    {
        History history = await GetStaffHistoryAsync(request.StaffId, context.CancellationToken);

        _dbContext.Entry(history).Property(e => e.MentorId).CurrentValue = request.Mentor;
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new Empty();
    }

    private async Task<Models.Staff> GetStaffAsync(int staffId, CancellationToken cancellationToken)
    {
        Models.Staff? staff = await _dbContext.Staff.FirstOrDefaultAsync(e => e.Id == staffId, cancellationToken);
        if (staff is null)
        {
            throw new NotFoundException($"Staff with id {staffId} not found");
        }

        return staff;
    }

    private async Task<History> GetStaffHistoryAsync(int staffId, CancellationToken cancellationToken)
    {
        History? history = await _dbContext.History.FirstOrDefaultAsync(e => e.StaffId == staffId, cancellationToken);
        if (history is null)
        {
            throw new NotFoundException($"Staff history with id {staffId} not found");
        }

        return history;
    }
}