using System;
using System.Threading.Tasks;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Models;
using EMS.Auth.API.Models.ResponseModels;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Utils.DateTimeUtil;
using Microsoft.EntityFrameworkCore;

namespace EMS.Auth.API.Services
{
    public class UsersService : BaseService<UsersService>, IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository, IEMSLogger<UsersService> logger, IDateTimeUtil dateTimeUtil) : base(logger, dateTimeUtil)
        {
            _usersRepository = usersRepository;
        }

        public virtual async Task<BaseResponse> AddAsync(User user)
        {
            try
            {
                int result = await _usersRepository.AddAsync(user);
                if (result == 0)
                {
                    throw new Exception($"User with login {user.Login} was not saved");
                }
                BaseResponse response = new()
                {
                    Id = user.Id,
                    ErrorMessage = string.Empty,
                    IsSucess = true
                };
                LogData log = new()
                {
                    CallSide = nameof(UsersService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = user,
                    Response = response
                };
                _logger.AddLog(log);
                return response;
            }
            catch (ArgumentException aex)
            {
                LogData log = new()
                {
                    CallSide = nameof(UsersService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = user,
                    Response = aex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    IsSucess = false,
                    ErrorMessage = aex.Message
                };
            }
            catch (InvalidOperationException ioex)
            {
                LogData log = new()
                {
                    CallSide = nameof(UsersService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = user,
                    Response = ioex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    IsSucess = false,
                    ErrorMessage = ioex.Message
                };
            }
            catch (DbUpdateException duex)
            {
                LogData log = new()
                {
                    CallSide = nameof(UsersService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = user,
                    Response = duex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    IsSucess = false,
                    ErrorMessage = "An error occured while saving user"
                };
            }
            catch (Exception ex)
            {
                LogData log = new()
                {
                    CallSide = nameof(UsersService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = user,
                    Response = ex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    IsSucess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public virtual async Task<BaseResponse> DeleteAsync(User user)
        {
            try
            {
                int result = await _usersRepository.DeleteAsync(user);
                if (result == 0)
                {
                    throw new Exception($"User with login {user.Login} was not deleted");
                }
                BaseResponse response = new()
                {
                    Id = user.Id,
                    ErrorMessage = string.Empty,
                    IsSucess = true
                };
                LogData log = new()
                {
                    CallSide = nameof(UsersService),
                    CallerMethodName = nameof(DeleteAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = user,
                    Response = response
                };
                _logger.AddLog(log);
                return response;
            }
            catch (DbUpdateException duex)
            {
                LogData log = new()
                {
                    CallSide = nameof(UsersService),
                    CallerMethodName = nameof(DeleteAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = user,
                    Response = duex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    IsSucess = false,
                    ErrorMessage = "An error occured while deleting user"
                };
            }
            catch (Exception ex)
            {
                LogData log = new()
                {
                    CallSide = nameof(UsersService),
                    CallerMethodName = nameof(DeleteAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = user,
                    Response = ex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    IsSucess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public virtual UserResponse GetById(long id)
        {
            try
            {
                UserResponse response = new()
                {
                    Id = id,
                    IsSucess = true,
                    ErrorMessage = string.Empty
                };
                User user = _usersRepository.GetById(id);
                response.User = user;
                LogData log = new()
                {
                    CallSide = nameof(UsersService),
                    CallerMethodName = nameof(GetById),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = id,
                    Response = response
                };
                _logger.AddLog(log);
                return response;
            }
            catch (Exception ex)
            {
                LogData log = new()
                {
                    CallSide = nameof(UsersService),
                    CallerMethodName = nameof(GetById),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = id,
                    Response = ex
                };
                _logger.AddErrorLog(log);
                return new UserResponse
                {
                    Id = id,
                    ErrorMessage = ex.Message,
                    IsSucess = false,
                    User = null
                };
            }
        }

        public virtual async Task<BaseResponse> UpdateAsync(User user)
        {
            try
            {
                int result = await _usersRepository.UpdateAsync(user);
                if (result == 0)
                {
                    throw new Exception($"User with login {user.Login} was not updated");
                }
                BaseResponse response = new()
                {
                    Id = user.Id,
                    ErrorMessage = string.Empty,
                    IsSucess = true
                };
                LogData log = new()
                {
                    CallSide = nameof(UsersService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = user,
                    Response = response
                };
                _logger.AddLog(log);
                return response;
            }
            catch (ArgumentException aex)
            {
                LogData log = new()
                {
                    CallSide = nameof(UsersService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = user,
                    Response = aex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    IsSucess = false,
                    ErrorMessage = aex.Message
                };
            }
            catch (DbUpdateException duex)
            {
                LogData log = new()
                {
                    CallSide = nameof(UsersService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = user,
                    Response = duex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    IsSucess = false,
                    ErrorMessage = "An error occured while updating user"
                };
            }
            catch (Exception ex)
            {
                LogData log = new()
                {
                    CallSide = nameof(UsersService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = user,
                    Response = ex
                };
                _logger.AddErrorLog(log);
                return new BaseResponse
                {
                    IsSucess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
