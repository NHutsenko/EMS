using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace EMS.Core.API.Services
{
    public class StaffService: Staffs.StaffsBase
    {
        private readonly IEMSLogger<StaffService> _logger;
        private readonly IStaffRepository _staffRepository;
        private readonly IDateTimeUtil _dateTimeUtil;

        public StaffService(IStaffRepository staffRepository, IEMSLogger<StaffService> logger, IDateTimeUtil dateTimeUtil)
        {
            _staffRepository = staffRepository;
            _logger = logger;
            _dateTimeUtil = dateTimeUtil;
        }

        public override async Task<BaseResponse> AddAsync(StaffData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _staffRepository.AddAsync(null);
                Staff staff = FromRpcModel(request);
                int result = await _staffRepository.AddAsync(staff);
                if(result == 0)
                {
                    throw new Exception("Staff has not been saved");
                }

                BaseResponse response = new BaseResponse
                {
                    Code = Code.Success,
                    DataId = staff.Id,
                    ErrorMessage = string.Empty
                };
                LogData logData = new LogData
                {
                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };
                _logger.AddLog(logData);

                return response;
            }
            catch(NullReferenceException nrex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = nrex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = nrex.Message
                };
            }
            catch(ArgumentException aex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = aex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = aex.Message
                };
            }
            catch(DbUpdateException duex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving staff data"
                };
            }
            catch(Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(AddAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.UnknownError,
                    ErrorMessage = ex.Message
                };
            }
        }

        public override async Task<BaseResponse> UpdateAsync(StaffData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _staffRepository.UpdateAsync(null);
                Staff staff = FromRpcModel(request);
                int result = await _staffRepository.UpdateAsync(staff);
                if (result == 0)
                {
                    throw new Exception("Staff has not been updated");
                }

                BaseResponse response = new BaseResponse
                {
                    Code = Code.Success,
                    DataId = staff.Id,
                    ErrorMessage = string.Empty
                };
                LogData logData = new LogData
                {
                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };
                _logger.AddLog(logData);

                return response;
            }
            catch (NullReferenceException nrex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = nrex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = nrex.Message
                };
            }
            catch (ArgumentException aex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = aex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = aex.Message
                };
            }
            catch (DbUpdateException duex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while updating staff"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(UpdateAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.UnknownError,
                    ErrorMessage = ex.Message
                };
            }
        }

        public override async Task<BaseResponse> DeleteAsync(StaffData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _staffRepository.DeleteAsync(null);
                Staff staff = FromRpcModel(request);
                int result = await _staffRepository.DeleteAsync(staff);
                if (result == 0)
                {
                    throw new Exception("Staff has not been deleted");
                }

                BaseResponse response = new BaseResponse
                {
                    Code = Code.Success,
                    DataId = staff.Id,
                    ErrorMessage = string.Empty
                };
                LogData logData = new LogData
                {
                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(DeleteAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };
                _logger.AddLog(logData);

                return response;
            }
            catch (NullReferenceException nrex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(DeleteAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = nrex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = nrex.Message
                };
            }
            catch (InvalidOperationException ioex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(DeleteAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ioex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DataError,
                    ErrorMessage = ioex.Message
                };
            }
            catch (DbUpdateException duex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(DeleteAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = duex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while deleting staff"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {
                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(DeleteAsync),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.UnknownError,
                    ErrorMessage = ex.Message
                };
            }
        }

        public override Task<StaffResponse> GetAll(Empty request, ServerCallContext context)
        {
            StaffResponse response = new StaffResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            try
            {
                IQueryable<Staff> staff = _staffRepository.GetAll();

                foreach(Staff s in staff)
                {
                    response.Data.Add(ToRpcModel(s));
                }

                LogData logData = new LogData
                {

                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(GetAll),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };
                _logger.AddLog(logData);
            }
            catch(Exception ex)
            {
                LogData logData = new LogData
                {

                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(GetAll),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                response.Status.Code = Code.UnknownError;
                response.Status.ErrorMessage = "An error occured while loading work periods data";
            }

            return Task.FromResult(response);
        }

        public override Task<StaffResponse> GetByManagerId(ByPersonIdRequest request, ServerCallContext context)
        {
            StaffResponse response = new StaffResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            try
            {
                IQueryable<Staff> staff = _staffRepository.GetByManagerId(request.PersonId);

                foreach (Staff s in staff)
                {
                    response.Data.Add(ToRpcModel(s));
                }

                LogData logData = new LogData
                {

                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(GetByManagerId),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };
                _logger.AddLog(logData);
            }
            catch(Exception ex)
            {
                LogData logData = new LogData
                {

                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(GetByManagerId),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                response.Status.Code = Code.UnknownError;
                response.Status.ErrorMessage = "An error occured while loading work periods data";
            }

            return Task.FromResult(response);
        }

        public override Task<StaffResponse> GetByPersonId(ByPersonIdRequest request, ServerCallContext context)
        {
            StaffResponse response = new StaffResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            try
            {
                IQueryable<Staff> staff = _staffRepository.GetByPersonId(request.PersonId);

                foreach (Staff s in staff)
                {
                    response.Data.Add(ToRpcModel(s));
                }

                LogData logData = new LogData
                {

                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(GetByPersonId),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = response
                };
                _logger.AddLog(logData);
            }
            catch (Exception ex)
            {
                LogData logData = new LogData
                {

                    CallSide = nameof(StaffService),
                    CallerMethodName = nameof(GetByPersonId),
                    CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                    Request = request,
                    Response = ex
                };
                _logger.AddErrorLog(logData);
                response.Status.Code = Code.UnknownError;
                response.Status.ErrorMessage = "An error occured while loading work periods data";
            }

            return Task.FromResult(response);
        }

        private static StaffData ToRpcModel(Staff staff)
        {
            return new StaffData
            {
                Id = staff.Id,
                CreatedOn = Timestamp.FromDateTime(staff.CreatedOn),
                ManagerId = staff.ManagerId,
                PersonId = staff.PersonId ?? 0,
                MotivationModificatorId = staff.MotivationModificatorId ?? 0,
                PositionId = staff.PositionId
            };
        }

        private static Staff FromRpcModel(StaffData staff)
        {
            return new Staff
            {
                Id = staff.Id,
                CreatedOn = staff.CreatedOn.ToDateTime(),
                ManagerId = staff.ManagerId,
                PersonId = staff.PersonId == 0 ? null : staff.PersonId,
                MotivationModificatorId = staff.MotivationModificatorId == 0 ? null : staff.MotivationModificatorId,
                PositionId = staff.PositionId
            };
        }
    }
}
