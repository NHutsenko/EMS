using EMS.Common.Models.BaseModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LoggerExtensions
{
    public static class LoggerExtensions
	{
        public static void AddLog<T>(this ILogger<T> logger, RequestResponseObject requestResponseObject)
        {
            string log = JsonConvert.SerializeObject(requestResponseObject);
            logger.LogInformation(log);
        }

        public static void AddErrorLog<T>(this ILogger<T> logger, RequestResponseObject requestResponseObject)
        {
            string log = JsonConvert.SerializeObject(requestResponseObject);
            logger.LogError(log);
        }
    }
}
