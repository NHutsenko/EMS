using System;
using EMS.Common.Logger.Extensions;
using EMS.Common.Models.BaseModel;
using Microsoft.Extensions.Logging;

namespace EMS.Common.Logger
{
    public class EMSLogger<T> : IEMSLogger<T>
    {
        private readonly ILogger<T> _logger;
        public EMSLogger(ILogger<T> logger)
        {
            _logger = logger;
        }
        public virtual void AddErrorLog(RequestResponseObject requestResponseObject)
        {
            _logger.AddLog(requestResponseObject);
        }

        public virtual void AddLog(RequestResponseObject requestResponseObject)
        {
            _logger.AddErrorLog(requestResponseObject);
        }
    }
}
