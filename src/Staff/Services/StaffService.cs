using EMS.Exceptions;
using EMS.Protos;
using EMS.Staff.Context;
using EMS.Staff.Interfaces;
using EMS.Staff.Models;
using Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace EMS.Staff.Services;

public sealed class StaffService : Protos.StaffService.StaffServiceBase
{
    private readonly IStaffRepository _staffRepository;

    public StaffService(IStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    public override async Task<Protos.Staff> GetByHistoryId(Int32Value request, ServerCallContext context)
    {
        Models.Staff data = await _staffRepository.GetByHistoryIdAsync(request.Value, context.CancellationToken);

        return new Protos.Staff
        {
            Id = data.Id,
            Position = data.PositionId,
            Manager = data.ManagerId,
            History = data.History == null
                ? null
                : new StaffHistory
                {
                    Person = data.History.PersonId,
                    Mentor = data.History.MentorId,
                    Employment = data.History.Employment,
                    CreatedOn = DateTime.SpecifyKind(data.History.CreatedOn, DateTimeKind.Utc).ToTimestamp()
                }
        };
    }

    public override async Task GetByPerson(Int32Value request, IServerStreamWriter<Protos.Staff> responseStream, ServerCallContext context)
    {
        IEnumerable<Protos.Staff> data = (await _staffRepository.GetByPersonAsync(request.Value, context.CancellationToken))
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
        await responseStream.WriteAllAsync(data);
    }

    public override async Task GetByManager(Int32Value request, IServerStreamWriter<Protos.Staff> responseStream, ServerCallContext context)
    {
        IEnumerable<Protos.Staff> data = (await _staffRepository.GetByManagerAsync(request.Value, context.CancellationToken))
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
        await responseStream.WriteAllAsync(data);
    }

    public override async Task GetAll(Empty request, IServerStreamWriter<Protos.Staff> responseStream, ServerCallContext context)
    {
        IEnumerable<Protos.Staff> data = (await _staffRepository.GetAllAsync(context.CancellationToken))
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

        await responseStream.WriteAllAsync(data);
    }

    public override async Task<Int32Value> Create(NewStaff request, ServerCallContext context)
    {
        int id = await _staffRepository.CreateAsync(request.Position, request.Manager, context.CancellationToken);

        return new Int32Value
        {
            Value = id
        };
    }

    public override async Task<Empty> CreateHistory(NewHistory request, ServerCallContext context)
    {
        await _staffRepository.CreateHistoryAsync(request.StaffId, 
            request.Data.Person,
            request.Data.Mentor,
            request.Data.Employment,
            request.Data.CreatedOn.ToDateTime(),
            context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> SetManager(NewManager request, ServerCallContext context)
    {
        await _staffRepository.SetManagerAsync(request.StaffId, request.Manager, context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> SetPosition(NewPosition request, ServerCallContext context)
    {
        await _staffRepository.SetPositionAsync(request.StaffId, request.Position, context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> SetDate(NewDate request, ServerCallContext context)
    {
        await _staffRepository.SetDateAsync(request.StaffId, request.Date.ToDateTime(), context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> SetEmployment(NewEmployment request, ServerCallContext context)
    {
        await _staffRepository.SetEmploymentAsync(request.StaffId, request.Employment, context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> SetMentor(NewMentor request, ServerCallContext context)
    {
        await _staffRepository.SetMentorAsync(request.StaffId, request.Mentor, context.CancellationToken);

        return new Empty();
    }
}