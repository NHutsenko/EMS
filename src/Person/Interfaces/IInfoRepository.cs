using EMS.Person.Models;

namespace EMS.Person.Interfaces;

public interface IInfoRepository
{
    Task<int> AddPersonInfoAsync(string lastName, string firstName, string login, string about, bool gender, DateTime bornOn, CancellationToken cancellationToken);
    Task UpdateNamesAsync(int id, string firstName, string lastName, CancellationToken cancellationToken);
    Task UpdateGenderAsync(int id, bool gender, CancellationToken cancellationToken);
    Task UpdateLoginAsync(int id, string login, CancellationToken cancellationToken);
}