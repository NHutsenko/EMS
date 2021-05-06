using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;

namespace EMS.Auth.API.Services
{
    public class BaseService<T>
    {
        protected readonly IEMSLogger<T> _logger;
        protected readonly IDateTimeUtil _dateTimeUtil;

        public BaseService(IEMSLogger<T> logger, IDateTimeUtil dateTimeUtil)
        {
            _logger = logger;
            _dateTimeUtil = dateTimeUtil;
        }
    }
}
