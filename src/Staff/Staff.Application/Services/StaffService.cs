using EMS.Staff.Application.Interfaces;
using EMS.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;

namespace EMS.Staff.Application.Services;

public sealed class StaffService : Protos.StaffService.StaffServiceBase
{
    private readonly IStaffRepository _staffRepository;
    private readonly IPeopleRepository _peopleRepository;
    private readonly IPositionRepository _positionRepository;

    public StaffService(IStaffRepository staffRepository, IPeopleRepository peopleRepository, IPositionRepository positionRepository)
    {
        _staffRepository = staffRepository;
        _peopleRepository = peopleRepository;
        _positionRepository = positionRepository;
    }

    public override async Task<Protos.Staff> GetById(Int32Value request, ServerCallContext context)
    {
        Domain.Staff data = await _staffRepository.GetByIdAsync(request.Value, context.CancellationToken);

        return new Protos.Staff
        {
            Id = data.Id,
            Position = data.PositionId,
            Manager = data.ManagerId,
            Person = data.History?.PersonId,
            Mentor = data.History?.MentorId,
            Employment = data.History?.Employment,
            StartWork = DateTime.SpecifyKind(data.History.CreatedOn, DateTimeKind.Utc).ToTimestamp()
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
                Person = e.History.PersonId,
                Mentor = e.History.MentorId,
                Employment = e.History.Employment,
                StartWork = DateTime.SpecifyKind(e.History.CreatedOn, DateTimeKind.Utc).ToTimestamp()
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
                Person = e.History.PersonId,
                Mentor = e.History.MentorId,
                Employment = e.History.Employment,
                StartWork = DateTime.SpecifyKind(e.History.CreatedOn, DateTimeKind.Utc).ToTimestamp()
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
                Person = e.History.PersonId,
                Mentor = e.History.MentorId,
                Employment = e.History.Employment,
                StartWork = DateTime.SpecifyKind(e.History.CreatedOn, DateTimeKind.Utc).ToTimestamp()
            });

        await responseStream.WriteAllAsync(data);
    }

    public override async Task<Int32Value> Create(NewStaff request, ServerCallContext context)
    {
        await _peopleRepository.CheckPeopleAsync(request.Person, request.Manager, request.Mentor, context.CancellationToken);
        await _positionRepository.ThrowExceptionIfPositionNotFoundAsync(request.Position, context.CancellationToken);
        await _staffRepository.ThrowExceptionIfDateIsWrongAsync(request.Person, request.StartWork.ToDateTime(), context.CancellationToken);
        
        int id = await _staffRepository.CreateAsync(request.Position, request.Manager, request.Person, request.Employment, request.StartWork.ToDateTime(), request.Mentor, context.CancellationToken);
        
        return new Int32Value
        {
            Value = id
        };
    }

    public override async Task<Empty> Edit(Protos.Staff request, ServerCallContext context)
    {
        Domain.Staff history = await _staffRepository.GetByIdAsync(request.Id, context.CancellationToken);
            
        await _peopleRepository.CheckPeopleAsync(null, request.Manager, request.Mentor, context.CancellationToken);
        await _positionRepository.ThrowExceptionIfPositionNotFoundAsync(request.Position, context.CancellationToken);
        await _staffRepository.ThrowExceptionIfDateIsWrongAsync(history.History.PersonId, request.StartWork.ToDateTime(), context.CancellationToken);

        await SetManagerAsync(history, request.Manager, context.CancellationToken);
        await SetPositionAsync(history, request.Position, context.CancellationToken);
        await SetStartWorkAsync(history, request.StartWork.ToDateTime(), context.CancellationToken);
        await SetEmploymentAsync(history, request.Employment.Value, context.CancellationToken);
        await SetMentorAsync(history, request.Mentor, context.CancellationToken);

        return new Empty();
    }

    private async Task SetMentorAsync(Domain.Staff staff, int? mentor, CancellationToken cancellationToken)
    {
        if (staff.History.MentorId != mentor)
        {
            await _staffRepository.SetMentorAsync(staff.Id, mentor, cancellationToken);
        }
    }

    private async Task SetEmploymentAsync(Domain.Staff staff, int employment, CancellationToken cancellationToken)
    {
        if (staff.History.Employment != employment)
        {
            await _staffRepository.SetEmploymentAsync(staff.Id, employment, cancellationToken);
        }
    }

    private async Task SetStartWorkAsync(Domain.Staff staff, DateTime startWork, CancellationToken cancellationToken)
    {
        if (staff.History.CreatedOn != startWork)
        {
            await _staffRepository.SetDateAsync(staff.Id, startWork, cancellationToken);
        }
    }

    private async Task SetPositionAsync(Domain.Staff staff, int positionId, CancellationToken cancellationToken)
    {
        if (staff.PositionId !=positionId)
        {
            await _staffRepository.SetPositionAsync(staff.Id, positionId, cancellationToken);
        }
    }

    private async Task SetManagerAsync(Domain.Staff staff, int managerId, CancellationToken cancellationToken)
    {
        if (staff.ManagerId != managerId)
        {
            await _staffRepository.SetManagerAsync(staff.Id, managerId, cancellationToken);
        }
    }
}