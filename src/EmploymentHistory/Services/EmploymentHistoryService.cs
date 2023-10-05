using EMS.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;

namespace EMS.EmploymentHistory.Services;

public sealed class EmploymentHistoryService: Protos.EmploymentHistoryService.EmploymentHistoryServiceBase
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
                EmploymentId = e.Id,
                StartWork = e.History.CreatedOn,
                Employment = e.History.Employment
            });

        await responseStream.WriteAllAsync(reply);
    }
}