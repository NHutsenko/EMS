using System;
using System.Threading.Tasks;
using EMS.Common.Logger;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

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
            throw new NotImplementedException();
        }

        public override async Task<BaseResponse> DeleteAsync(OtherPaymentData request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override async Task<BaseResponse> UpdateAsync(OtherPaymentData request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<OtherPaymentsResponse> GetByPersonId(ByPersonIdRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<OtherPaymentsResponse> GetByPersonIdAndDateRange(ByPersonIdAndDateRangeRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
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
