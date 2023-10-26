using EMS.Exceptions;
using EMS.Protos;
using Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;

namespace EMS.EmploymentHistory.Services;

public sealed partial class EmploymentHistoryService : Protos.EmploymentHistoryService.EmploymentHistoryServiceBase
{
    private readonly PersonService.PersonServiceClient _personServiceClient;
    private readonly PositionService.PositionServiceClient _positionServiceClient;
    private readonly StaffService.StaffServiceClient _staffServiceClient;

    public EmploymentHistoryService(PersonService.PersonServiceClient personServiceClient,
        PositionService.PositionServiceClient positionServiceClient,
        StaffService.StaffServiceClient staffServiceClient)
    {
        _personServiceClient = personServiceClient;
        _positionServiceClient = positionServiceClient;
        _staffServiceClient = staffServiceClient;
    }

    public override async Task GetPersonEmployment(Int32Value request, IServerStreamWriter<EmploymentHistoryData> responseStream, ServerCallContext context)
    {
        AsyncServerStreamingCall<Staff>? staffCall = _staffServiceClient.GetByPerson(request);
        List<Staff>? staff = await staffCall.ResponseStream.ToListAsync();
        IEnumerable<EmploymentHistoryData> reply = staff.Select(e => new EmploymentHistoryData
        {
            ManagerId = e.Manager,
            PositionId = e.Position,
            EmploymentHistoryId = e.Id,
            StartWork = e.History.CreatedOn,
            Employment = e.History.Employment,
            MentorId = e.History.Mentor
        });

        await responseStream.WriteAllAsync(reply);
    }

    private async Task ThrowExceptionIfDateIsWrongAsync(int personId, Timestamp date)
    {
        Int32Value personStaffRequest = new()
        {
            Value = personId
        };
        AsyncServerStreamingCall<Staff>? call = _staffServiceClient.GetByPerson(personStaffRequest);
        List<Staff>? staff = await call.ResponseStream.ToListAsync();
        if (staff.Exists(e => e.History.CreatedOn > date))
        {
            DateTime minDate = staff.OrderByDescending(e => e.History.CreatedOn)
                .First()
                .History.CreatedOn.ToDateTime()
                .AddDays(1);
            throw new AlreadyExistsException($"Employment period for date {date.ToDateTime().Date} already exists." +
                                             $"{Environment.NewLine}Minimum start work date is {minDate.Date}");
        }
    }

    private async Task ThrowExceptionIfPositionNotFoundAsync(int positionId)
    {
        AsyncServerStreamingCall<Position>? call = _positionServiceClient.GetAll(new Empty());
        List<Position>? positions = await call.ResponseStream.ToListAsync();
        if (positions.Exists(e => e.Grades.Any(g => g.ActualHistoryId == positionId)) is false)
            throw new NotFoundException($"Position with history id {positionId} does not exists");
    }

    private async Task CheckPeopleAsync(int? personId, int managerId, int? mentorId)
    {
        if (personId.HasValue)
        {
            Int32Value personRequest = new()
            {
                Value = personId.Value
            };
            await _personServiceClient.GetAsync(personRequest);
        }

        Int32Value managerRequest = new()
        {
            Value = managerId
        };
        await _personServiceClient.GetAsync(managerRequest);

        if (mentorId.HasValue)
        {
            Int32Value mentorRequest = new()
            {
                Value = mentorId.Value
            };
            await _personServiceClient.GetAsync(mentorRequest);
        }
    }
}