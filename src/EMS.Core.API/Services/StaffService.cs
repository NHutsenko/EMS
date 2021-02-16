using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common.Logger;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

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

        public override Task<BaseResponse> AddAsync(StaffData request, ServerCallContext context)
        {
            return base.AddAsync(request, context);
        }

        public override Task<BaseResponse> UpdateAsync(StaffData request, ServerCallContext context)
        {
            return base.UpdateAsync(request, context);
        }

        public override Task<BaseResponse> DeleteAsync(StaffData request, ServerCallContext context)
        {
            return base.DeleteAsync(request, context);
        }

        public override Task<StaffResponse> GetAll(Empty request, ServerCallContext context)
        {
            return base.GetAll(request, context);
        }

        public override Task<StaffResponse> GetByManagerId(ByPersonIdRequest request, ServerCallContext context)
        {
            return base.GetByManagerId(request, context);
        }

        public override Task<StaffResponse> GetByPersonId(ByPersonIdRequest request, ServerCallContext context)
        {
            return base.GetByPersonId(request, context);
        }
    }
}
