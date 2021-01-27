using System.Threading.Tasks;
using EMS.Common.Logger;
using EMS.Common.Protos;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL.Repositories.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace EMS.Core.API.Services
{
    public class HolidaysService: Holidays.HolidaysBase
    {
        private readonly IHolidaysRepository _holidaysRepository;
        private readonly IEMSLogger<HolidaysService> _logger;
        private readonly IDateTimeUtil _dateTimeUtil;

        public HolidaysService(IHolidaysRepository holidaysRepository, IEMSLogger<HolidaysService> logger, IDateTimeUtil dateTimeUtil)
        {
            _holidaysRepository = holidaysRepository;
            _logger = logger;
            _dateTimeUtil = dateTimeUtil;
        }


        public override Task<BaseResponse> AddAsync(HolidayData request, ServerCallContext context)
        {
            return base.AddAsync(request, context);
        }

        public override Task<BaseResponse> DeleteAsync(HolidayData request, ServerCallContext context)
        {
            return base.DeleteAsync(request, context);
        }

        public override Task<BaseResponse> UpdateAsync(HolidayData request, ServerCallContext context)
        {
            return base.UpdateAsync(request, context);
        }

        public override Task<HolidaysResponse> GetAll(Empty request, ServerCallContext context)
        {
            return base.GetAll(request, context);
        }

        public override Task<HolidaysResponse> GetByDateRange(DateRangeRequest request, ServerCallContext context)
        {
            return base.GetByDateRange(request, context);
        }
    }
}
