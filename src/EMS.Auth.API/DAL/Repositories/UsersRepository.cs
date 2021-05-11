using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Models;
using EMS.Common.Utils.DateTimeUtil;

namespace EMS.Auth.API.DAL.Repositories
{
    public class UsersRepository : BaseRepository, IUsersRepository
    {

        public UsersRepository(IApplicationDbContext applicationDbContext, IDateTimeUtil dateTimeUtil) : base(applicationDbContext, dateTimeUtil) { }

        public virtual async Task<int> AddAsync(User user)
        {
            CheckData(user);
            if (_context.Users.Any(e => e.Login == user.Login))
            {
                throw new InvalidOperationException($"User with login '{user.Login}' already exists");
            }
            user.CreatedOn = _dateTimeUtil.GetCurrentDateTime();
            _context.Users.Add(user);
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync();
        }

        public virtual User GetById(long id)
        {
            return _context.Users.FirstOrDefault(e => e.Id == id);
        }

        public virtual User GetByLogin(string login)
        {
            return _context.Users.FirstOrDefault(e => e.Login == login);
        }

        public virtual User VerifyUser(string login, string password)
        {
            return _context.Users.FirstOrDefault(e => e.Login.Equals(login) && e.Password.Equals(password));
        }

        public virtual async Task<int> UpdateAsync(User user)
        {
            CheckData(user);
            _context.Users.Update(user);
            return await _context.SaveChangesAsync();
        }

        private static void CheckData(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Login))
            {
                throw new ArgumentException("Login cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException("Password cannot be empty");
            }
        }
    }
}
