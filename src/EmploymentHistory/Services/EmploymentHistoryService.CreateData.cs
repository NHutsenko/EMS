using EMS.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace EMS.EmploymentHistory.Services;

public sealed partial class EmploymentHistoryService
{
    public override async Task<Int32Value> CreateEmployment(NewEmploymentHistory request, ServerCallContext context)
    {
        await CheckPeopleAsync(request.PersonId, request.ManagerId, request.MentorId);
        await ThrowExceptionIfPositionNotFoundAsync(request.PositionId);
        await ThrowExceptionIfDateIsWrongAsync(request.PersonId, request.StartWork);

        NewStaff staff = new()
        {
            Position = request.PositionId,
            Manager = request.ManagerId
        };
        Int32Value staffId = await _staffServiceClient.CreateAsync(staff);

        NewHistory history = new()
        {
            StaffId = staffId.Value,
            Data = new StaffHistory
            {
                Person = request.PersonId,
                Mentor = request.MentorId,
                CreatedOn = request.StartWork,
                Employment = request.Employment
            }
        };
        await _staffServiceClient.CreateHistoryAsync(history);
        
        return staffId;
    }
}