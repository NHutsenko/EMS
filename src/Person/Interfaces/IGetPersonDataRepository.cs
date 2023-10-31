using EMS.Person.Models;

namespace EMS.Person.Interfaces;

public interface IGetPersonDataRepository
{
    Task<PersonInfo> GetPersonAsync(int personId, CancellationToken cancellationToken);
    Task<IEnumerable<PersonInfo>> GetPeopleAsync(CancellationToken cancellationToken);
}