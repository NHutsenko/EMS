using EMS.Exceptions;
using EMS.Person.Interfaces;
using EMS.Person.Models;
using EMS.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace EMS.Person.Services;

public sealed partial class PersonService
{
    private readonly IUpdatePersonDataRepository _updatePersonDataRepository;
    public override async Task<Empty> UpdateNames(NamesRequest request, ServerCallContext context)
    {
        await _updatePersonDataRepository.UpdateNamesAsync(request.Id, request.LastName, request.FirstName, context.CancellationToken);
        return new Empty();
    }

    public override async Task<Empty> UpdateGender(GenderRequest request, ServerCallContext context)
    {
        await _updatePersonDataRepository.UpdateGenderAsync(request.Id, request.Value, context.CancellationToken);
        return new Empty();
    }

    public override async Task<Empty> UpdateAddress(AddressRequest request, ServerCallContext context)
    {
        await _updatePersonDataRepository.UpdateAddressAsync(request.PersonId,
            request.Address.City,
            request.Address.Street,
            request.Address.Building,
            request.Address.House,
            context.CancellationToken);
        
        return new Empty();
    }

    public override async Task<Empty> UpdateAboutYourself(AboutRequest request, ServerCallContext context)
    {
        await _updatePersonDataRepository.UpdateAboutYourselfAsync(request.PersonId, request.Value, context.CancellationToken);

        return new Empty();
    }
}