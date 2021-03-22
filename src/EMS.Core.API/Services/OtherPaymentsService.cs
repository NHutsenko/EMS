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
    public class OtherPaymentsService : OtherPayments.OtherPaymentsBase
    {
        private readonly IOtherPaymentsRepository _otherPaymentsRepository;
        private readonly IDateTimeUtil _dateTimeUtil;
        private readonly IEMSLogger<OtherPaymentsService> _logger;

        public OtherPaymentsService(IOtherPaymentsRepository otherPaymentsRepository, IEMSLogger<OtherPaymentsService> logger, IDateTimeUtil dateTimeUtil)
        {
            _logger = logger;
            _dateTimeUtil = dateTimeUtil;
            _otherPaymentsRepository = otherPaymentsRepository;
        }

        public override async Task<BaseResponse> AddAsync(OtherPaymentData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _otherPaymentsRepository.AddAsync(null);
                OtherPayment otherPayment = FromRpcModel(request);
                int result = await _otherPaymentsRepository.AddAsync(otherPayment);
                if(result == 0)
                {
                    throw new Exception("Other payment has not been saved");
                }
                BaseResponse response = new()
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty,
                    DataId = otherPayment.Id
                };

                LogData logData = new()
				{
					CallSide = nameof(OtherPaymentsService),
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
					CallSide = nameof(OtherPaymentsService),
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
					CallSide = nameof(OtherPaymentsService),
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
                LogData logData = new()
				{
					CallSide = nameof(OtherPaymentsService),
					CallerMethodName = nameof(AddAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = duex
				};
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while saving other payment"
                };
            }
            catch(Exception ex)
            {
                LogData logData = new()
				{
					CallSide = nameof(OtherPaymentsService),
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

        public override async Task<BaseResponse> DeleteAsync(OtherPaymentData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _otherPaymentsRepository.DeleteAsync(null);

                OtherPayment otherPayment = FromRpcModel(request);
                int result = await _otherPaymentsRepository.DeleteAsync(otherPayment);
                if(result == 0)
                {
                    throw new Exception("Other payment has not been deleted");
                }
                BaseResponse response = new()
				{
					Code = Code.Success,
					ErrorMessage = string.Empty,
					DataId = otherPayment.Id
				};

                LogData logData = new()
				{
					CallSide = nameof(OtherPaymentsService),
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
                LogData logData = new()
				{
					CallSide = nameof(OtherPaymentsService),
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
                LogData logData = new()
				{
					CallSide = nameof(OtherPaymentsService),
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
                LogData logData = new()
				{
					CallSide = nameof(OtherPaymentsService),
					CallerMethodName = nameof(DeleteAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = duex
				};
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while deleting other payment"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new()
				{
					CallSide = nameof(OtherPaymentsService),
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

        public override async Task<BaseResponse> UpdateAsync(OtherPaymentData request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    await _otherPaymentsRepository.UpdateAsync(null);
                OtherPayment otherPayment = FromRpcModel(request);
                int result = await _otherPaymentsRepository.UpdateAsync(otherPayment);
                if (result == 0)
                {
                    throw new Exception("Other payment has not been updated");
                }
                BaseResponse response = new()
				{
					Code = Code.Success,
					ErrorMessage = string.Empty,
					DataId = otherPayment.Id
				};

                LogData logData = new()
				{
					CallSide = nameof(OtherPaymentsService),
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
					CallSide = nameof(OtherPaymentsService),
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
					CallSide = nameof(OtherPaymentsService),
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
					CallSide = nameof(OtherPaymentsService),
					CallerMethodName = nameof(UpdateAsync),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = duex
				};
                _logger.AddErrorLog(logData);
                return new BaseResponse
                {
                    Code = Code.DbError,
                    ErrorMessage = "An error occured while updating other payment"
                };
            }
            catch (Exception ex)
            {
                LogData logData = new()
				{
					CallSide = nameof(OtherPaymentsService),
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

        public override Task<OtherPaymentsResponse> GetByPersonId(ByPersonIdRequest request, ServerCallContext context)
        {
            OtherPaymentsResponse response = new OtherPaymentsResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            try
            {
                IQueryable<OtherPayment> otherPayments = _otherPaymentsRepository.GetByPersonId(request.PersonId);

                foreach(OtherPayment otherPayment in otherPayments)
                {
                    response.Data.Add(ToRpcModel(otherPayment));
                }

                LogData logData = new()
				{
					CallSide = nameof(OtherPaymentsService),
					CallerMethodName = nameof(GetByPersonId),
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
					CallSide = nameof(OtherPaymentsService),
					CallerMethodName = nameof(GetByPersonId),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = ex
				};
                _logger.AddErrorLog(logData);
                response.Status.Code = Code.UnknownError;
                response.Status.ErrorMessage = "An error ocured while loading other payments";
            }
            return Task.FromResult(response);
        }

        public override Task<OtherPaymentsResponse> GetByPersonIdAndDateRange(ByPersonIdAndDateRangeRequest request, ServerCallContext context)
        {
            OtherPaymentsResponse response = new OtherPaymentsResponse
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };

            try
            {
                IQueryable<OtherPayment> otherPayments = _otherPaymentsRepository.GetByPersonIdAndDateRange(request.Person.PersonId, request.Range.From.ToDateTime(), request.Range.To.ToDateTime());

                foreach (OtherPayment otherPayment in otherPayments)
                {
                    response.Data.Add(ToRpcModel(otherPayment));
                }

                LogData logData = new()
				{
					CallSide = nameof(OtherPaymentsService),
					CallerMethodName = nameof(GetByPersonIdAndDateRange),
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
					CallSide = nameof(OtherPaymentsService),
					CallerMethodName = nameof(GetByPersonIdAndDateRange),
					CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
					Request = request,
					Response = ex
				};
                _logger.AddErrorLog(logData);
                response.Status.Code = Code.UnknownError;
                response.Status.ErrorMessage = "An error ocured while loading other payments";
            }
            return Task.FromResult(response);
        }

        private static OtherPayment FromRpcModel(OtherPaymentData otherPaymentData)
        {
            return new OtherPayment
            {
                Id = otherPaymentData.Id,
                Comment = otherPaymentData.Comment,
                CreatedOn = otherPaymentData.CreatedOn.ToDateTime(),
                PersonId = otherPaymentData.PersonId,
                Value = otherPaymentData.Value
            };
        }

        private static OtherPaymentData ToRpcModel(OtherPayment otherPayment)
        {
            return new OtherPaymentData
            {
                Id = otherPayment.Id,
                Comment = otherPayment.Comment,
                CreatedOn = Timestamp.FromDateTime(otherPayment.CreatedOn),
                PersonId = otherPayment.PersonId,
                Value = otherPayment.Value
            };
        }
    }
}
