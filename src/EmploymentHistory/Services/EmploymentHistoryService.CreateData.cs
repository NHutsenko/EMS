using EMS.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace EMS.EmploymentHistory.Services;

public sealed partial class EmploymentHistoryService
{
    public override async Task<Int32Value> CreateEmployment(NewEmploymentHistory request, ServerCallContext context)
    {
        Int32Value personRequest = new()
        {
            Value = request.PersonId
        };
        await _personServiceClient.GetAsync(personRequest);
        
        Int32Value managerRequest = new()
        {
            Value = request.ManagerId
        };
        await _personServiceClient.GetAsync(managerRequest);

        return new Int32Value();
    }
}