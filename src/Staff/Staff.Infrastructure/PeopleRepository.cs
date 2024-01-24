using Google.Protobuf.WellKnownTypes;
using EMS.Protos;
using EMS.Staff.Application.Interfaces;

namespace EMS.Staff.Infrastructure;

public sealed class PeopleRepository: IPeopleRepository
{
    private readonly PersonService.PersonServiceClient _personServiceClient;

    public PeopleRepository(PersonService.PersonServiceClient personServiceClient)
    {
        _personServiceClient = personServiceClient;
    }

    public async Task CheckPeopleAsync(int? personId, int managerId, int? mentorId, CancellationToken cancellationToken)
    {
        if (personId.HasValue)
        {
            Int32Value personRequest = new()
            {
                Value = personId.Value
            };
            await _personServiceClient.GetAsync(personRequest, cancellationToken: cancellationToken);
        }

        Int32Value managerRequest = new()
        {
            Value = managerId
        };
        await _personServiceClient.GetAsync(managerRequest, cancellationToken: cancellationToken);

        if (mentorId.HasValue)
        {
            Int32Value mentorRequest = new()
            {
                Value = mentorId.Value
            };
            await _personServiceClient.GetAsync(mentorRequest, cancellationToken: cancellationToken);
        }
    }
}