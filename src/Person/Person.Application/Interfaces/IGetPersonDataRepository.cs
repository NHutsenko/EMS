using EMS.Person.Domain;

namespace EMS.Person.Application.Interfaces;

public interface IGetPersonDataRepository
{
    Task<PersonInfo> GetPersonAsync(int personId, CancellationToken cancellationToken);
    Task<IEnumerable<PersonInfo>> GetPeopleAsync(CancellationToken cancellationToken);
}