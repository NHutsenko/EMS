using EMS.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace EMS.EmploymentHistory.Services;

public sealed partial class EmploymentHistoryService
{
    public override async Task<Empty> UpdateEmployment(EmploymentHistoryData request, ServerCallContext context)
    {
        Int32Value staffRequest = new()
        {
            Value = request.EmploymentHistoryId
        };
        Staff staff = await _staffServiceClient.GetByHistoryIdAsync(staffRequest);
            
        await CheckPeopleAsync(null, request.ManagerId, request.MentorId);
        await ThrowExceptionIfPositionNotFoundAsync(request.PositionId);
        await ThrowExceptionIfDateIsWrongAsync(staff.History.Person, request.StartWork);

        await SetManagerAsync(staff, request.ManagerId);
        await SetPositionAsync(staff, request.PositionId);
        await SetStartWorkAsync(staff, request.StartWork);
        await SetEmploymentAsync(staff, request.Employment);
        await SetMentorAsync(staff, request.MentorId);
        
        return new Empty();
    }

    private async Task SetMentorAsync(Staff staff, int? mentor)
    {
        if (staff.History.Mentor != mentor)
        {
            NewMentor mentorRequest = new()
            {
                StaffId = staff.Id,
                Mentor = mentor
            };
            await _staffServiceClient.SetMentorAsync(mentorRequest);
        }
    }

    private async Task SetEmploymentAsync(Staff staff, int employment)
    {
        if (staff.History.Employment != employment)
        {
            NewEmployment employmentRequest = new()
            {
                StaffId = staff.Id,
                Employment = employment
            };
            await _staffServiceClient.SetEmploymentAsync(employmentRequest);
        }
    }

    private async Task SetStartWorkAsync(Staff staff, Timestamp startWork)
    {
        if (staff.History.CreatedOn != startWork)
        {
            NewDate startWorkRequest = new()
            {
                StaffId = staff.Id,
                Date = startWork
            };
            await _staffServiceClient.SetDateAsync(startWorkRequest);
        }
    }

    private async Task SetPositionAsync(Staff staff, int positionId)
    {
        if (staff.Position !=positionId)
        {
            NewPosition positionRequest = new()
            {
                StaffId = staff.Id,
                Position = positionId
            };
            await _staffServiceClient.SetPositionAsync(positionRequest);
        }
    }

    private async Task SetManagerAsync(Staff staff, int managerId)
    {
        if (staff.Manager != managerId)
        {
            NewManager managerRequest = new()
            {
                StaffId = staff.Id,
                Manager = managerId
            };
            await _staffServiceClient.SetManagerAsync(managerRequest);
        }
    }
}