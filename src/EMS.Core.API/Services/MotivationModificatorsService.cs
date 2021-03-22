using System;
using System.Collections.Generic;
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
    public class MotivationModificatorsService: MotivationModificators.MotivationModificatorsBase
    {
        private readonly IMotivationModificatorRepository _motivationModificatorRepository;
        private readonly IEMSLogger<MotivationModificatorsService> _logger;
        private readonly IDateTimeUtil _dateTimeUtil;

        public MotivationModificatorsService(IMotivationModificatorRepository motivationModificatorRepository, 
            IEMSLogger<MotivationModificatorsService> logger,
            IDateTimeUtil dateTimeUtil)
        {
            _motivationModificatorRepository = motivationModificatorRepository;
            _logger = logger;
            _dateTimeUtil = dateTimeUtil;
        }

        public override async Task<BaseResponse> AddAsync(MotivationModificatorData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _motivationModificatorRepository.AddAsync(null);

                MotivationModificator motivationModificator = FromRpcModel(request);
                int result = await _motivationModificatorRepository.AddAsync(motivationModificator);
                if(result == 0)
                {
                    throw new Exception("Motivation modificator has not been saved");
                }

                BaseResponse response = new()
                {
                    Code = Code.Success,
                    DataId = motivationModificator.Id,
                    ErrorMessage = string.Empty
                };

                LogData logData = new()
				{
					CallSide = nameof(MotivationModificatorsService),
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
                LogData logData = new()
				{
					CallSide = nameof(MotivationModificatorsService),
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
                LogData logData = new()
				{
					CallSide = nameof(MotivationModificatorsService),
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
            catch(InvalidOperationException ioex)
            {
                LogData logData = new()
				{
					CallSide = nameof(MotivationModificatorsService),
					CallerMethodName = nameof(AddAsync),
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
            catch(DbUpdateException duex)
            {
                LogData logData = new()
				{
					CallSide = nameof(MotivationModificatorsService),
					CallerMethodName = nameof(AddAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = duex
				};
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving motivation modificator"
                };
            }
            catch(Exception ex)
            {
                LogData logData = new()
				{
					CallSide = nameof(MotivationModificatorsService),
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

        public override async Task<BaseResponse> UpdateAsync(MotivationModificatorData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _motivationModificatorRepository.UpdateAsync(null);

                MotivationModificator motivationModificator = FromRpcModel(request);
                int result = await _motivationModificatorRepository.UpdateAsync(motivationModificator);
                if (result == 0)
                {
                    throw new Exception("Motivation modificator has not been updated");
                }

                BaseResponse response = new()
				{
					Code = Code.Success,
					DataId = motivationModificator.Id,
					ErrorMessage = string.Empty
				};

                LogData logData = new()
				{
					CallSide = nameof(MotivationModificatorsService),
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
                LogData logData = new()
				{
					CallSide = nameof(MotivationModificatorsService),
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
                LogData logData = new()
				{
					CallSide = nameof(MotivationModificatorsService),
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
                LogData logData = new()
				{
					CallSide = nameof(MotivationModificatorsService),
					CallerMethodName = nameof(UpdateAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = duex
				};
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while updating motivation modificator"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new()
				{
					CallSide = nameof(MotivationModificatorsService),
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

        public override Task<MotivationModificatorResponse> GetByStaffId(ByStaffIdRequest request, ServerCallContext context)
        {
            MotivationModificatorResponse response = new()
			{
				Status = new BaseResponse { Code = Code.Success, ErrorMessage = string.Empty }
			};
            try
            { 
                MotivationModificator motivationModificator = _motivationModificatorRepository.GetByStaffId(request.StaffId);
                response.Data = motivationModificator is null ? null: ToRpcModel(motivationModificator);

                LogData logData = new()
				{
					CallSide = nameof(MotivationModificatorsService),
					CallerMethodName = nameof(GetByStaffId),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = response
				};
                _logger.AddLog(logData);
            }
            catch(Exception ex)
            {
                LogData logData = new()
				{
					CallSide = nameof(MotivationModificatorsService),
					CallerMethodName = nameof(GetByStaffId),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = ex
				};
                _logger.AddErrorLog(logData);
                response.Status.Code = Code.UnknownError;
                response.Status.ErrorMessage = "An error occured while loading motivation modificator data";
            }
            return Task.FromResult(response);
        }

        private static MotivationModificatorData ToRpcModel(MotivationModificator motivationModificator)
        {
            return new MotivationModificatorData
            {
                Id = motivationModificator.Id,
                StaffId = motivationModificator.StaffId,
                ModValue = motivationModificator.ModValue,
                CreatedOn = Timestamp.FromDateTime(motivationModificator.CreatedOn)
            };
        }

        private static MotivationModificator FromRpcModel(MotivationModificatorData motivationModificator)
        {
            return new MotivationModificator
            {
                Id = motivationModificator.Id,
                StaffId = motivationModificator.StaffId,
                ModValue = motivationModificator.ModValue,
                CreatedOn = motivationModificator.CreatedOn == null ? DateTime.MinValue :  motivationModificator.CreatedOn.ToDateTime()
            };
        }
    }
}
