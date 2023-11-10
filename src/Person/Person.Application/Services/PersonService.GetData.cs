using EMS.Person.Application.Interfaces;
using EMS.Person.Application.Mappers;
using EMS.Person.Domain;
using EMS.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;

namespace EMS.Person.Application.Services;

public sealed partial class PersonService : Protos.PersonService.PersonServiceBase
{
    private readonly IGetPersonDataRepository _getPersonDataRepository;

    public PersonService(IGetPersonDataRepository getPersonDataRepository, IAddPersonDataRepository addPersonDataRepository, IUpdatePersonDataRepository updatePersonDataRepository)
    {
        _getPersonDataRepository = getPersonDataRepository;
        _updatePersonDataRepository = updatePersonDataRepository;
        _addPersonDataRepository = addPersonDataRepository;
    }

    public override async Task<PersonData?> Get(Int32Value request, ServerCallContext context)
    {
        PersonInfo data = await _getPersonDataRepository.GetPersonAsync(request.Value, context.CancellationToken);
        
        return data.MapToProto();
    }

    public override async Task GetAll(Empty request, IServerStreamWriter<PersonData> responseStream, ServerCallContext context)
    {
        IEnumerable<PersonData> data = (await _getPersonDataRepository.GetPeopleAsync(context.CancellationToken))
            .Select(e => e.MapToProto());

        await responseStream.WriteAllAsync(data);
    }
}