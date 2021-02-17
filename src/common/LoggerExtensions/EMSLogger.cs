using System.Diagnostics.CodeAnalysis;
using EMS.Common.Logger.Extensions;
using EMS.Common.Logger.Models;
using Microsoft.Extensions.Logging;

namespace EMS.Common.Logger
{
    [ExcludeFromCodeCoverage]
    public class EMSLogger<T> : IEMSLogger<T>
    {
        private readonly ILogger<T> _logger;
        public EMSLogger(ILogger<T> logger)
        {
            _logger = logger;
        }
        public virtual void AddErrorLog(LogData requestResponseObject)
        {
            _logger.AddLog(requestResponseObject);
        }

        public virtual void AddLog(LogData requestResponseObject)
        {
            _logger.AddErrorLog(requestResponseObject);
        }
    }
}
