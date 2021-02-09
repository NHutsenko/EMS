using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Common.Logger;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using Grpc.Core;

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

        public override Task<BaseResponse> AddAsync(MotivationModificatorData request, ServerCallContext context)
        {
            return base.AddAsync(request, context);
        }

        public override Task<BaseResponse> UpdateAsync(MotivationModificatorData request, ServerCallContext context)
        {
            return base.UpdateAsync(request, context);
        }

        public override Task<MotivationModificatorsResponse> GetByStaffId(ByStaffIdRequest request, ServerCallContext context)
        {
            return base.GetByStaffId(request, context);
        }
    }
}
