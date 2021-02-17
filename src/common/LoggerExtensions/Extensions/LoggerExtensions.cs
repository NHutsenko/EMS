using System.Diagnostics.CodeAnalysis;
using EMS.Common.Logger.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EMS.Common.Logger.Extensions
{
    [ExcludeFromCodeCoverage]
    internal static class LoggerExtensions
	{
        public static void AddLog<T>(this ILogger<T> logger, LogData requestResponseObject)
        {
            string log = JsonConvert.SerializeObject(requestResponseObject);
            logger.LogInformation(log);
        }

        public static void AddErrorLog<T>(this ILogger<T> logger, LogData requestResponseObject)
        {
            string log = JsonConvert.SerializeObject(requestResponseObject);
            logger.LogError(log);
        }
    }
}
