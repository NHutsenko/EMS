using System.Linq;
using System.Threading.Tasks;
using EMS.Core.API.Models;

namespace EMS.Core.API.DAL.Repositories.Interfaces
{
    public interface IPeopleRepository
    {
        public IQueryable<Person> GetAll();
        public Person GetById(long personId);
        Task<int> AddAsync(Person person);
        Task<int> UpdateAsync(Person person);
        Task<int> AddPhotoAsync(PersonPhoto photo);
        Task<int> AddContactAsync(Contact photo);
    }
}
