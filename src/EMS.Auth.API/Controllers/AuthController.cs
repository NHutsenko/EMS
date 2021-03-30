using EMS.Common.ControllerExtension;
using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;

namespace EMS.Auth.API.Controllers
{
    public class AuthController : BaseApiController<AuthController>
    {
        public AuthController(IEMSLogger<AuthController> logger, IDateTimeUtil dateTimeUtil) :
            base(logger, dateTimeUtil)
        { }
    }
}
