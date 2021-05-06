using System;
using System.Threading.Tasks;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Models;
using EMS.Auth.API.Models.ResponseModels;
using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;
using Microsoft.EntityFrameworkCore;

namespace EMS.Auth.API.Services
{
    public class UsersService : BaseService<UsersService>, IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository, IEMSLogger<UsersService> logger, IDateTimeUtil dateTimeUtil): base(logger, dateTimeUtil)
        {
            _usersRepository = usersRepository;
        }

        public virtual Task<BaseResponse> AddAsync(User user)
        {
            throw new NotImplementedException();
            try
            {

            }
            catch(ArgumentException aex)
            {

            }
            catch(InvalidOperationException ioex)
            {

            }
            catch(DbUpdateException duex)
            {

            }
            catch(Exception ex)
            {

            }
        }

        public virtual Task<BaseResponse> DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public User GetById(long id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<BaseResponse> UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
