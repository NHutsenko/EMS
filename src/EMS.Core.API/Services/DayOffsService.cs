using System.Threading.Tasks;
using EMS.Common.Logger;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using Grpc.Core;

namespace EMS.Core.API.Services
{
    public class DayOffsService: DayOffs.DayOffsBase
    {
        private readonly IDayOffRepository _dayOffRepository;
        private readonly IEMSLogger<DayOffsService> _logger;
        private readonly IDateTimeUtil _dateTimeUtil;

        public DayOffsService(IDayOffRepository dayOffRepository, IEMSLogger<DayOffsService> logger, IDateTimeUtil dateTimeUtil)
        {
            _dateTimeUtil = dateTimeUtil;
            _dayOffRepository = dayOffRepository;
            _logger = logger;
        }

        public override Task<BaseResponse> AddAsync(DayOffData request, ServerCallContext context)
        {
            return base.AddAsync(request, context);
        }

        public override Task<BaseResponse> DeleteAsync(DayOffData request, ServerCallContext context)
        {
            return base.DeleteAsync(request, context);
        }

        public override Task<BaseResponse> UpdateAsync(DayOffData request, ServerCallContext context)
        {
            return base.UpdateAsync(request, context);
        }

        public override Task<DayOffsResponse> GetByPersonId(DayOffsByPersonRequest request, ServerCallContext context)
        {
            return base.GetByPersonId(request, context);
        }

        public override Task<DayOffsResponse> GetByPersonIdAndDateRange(DayOffsByDateRangeRequestRequest request, ServerCallContext context)
        {
            return base.GetByPersonIdAndDateRange(request, context);
        }
    }
}
