using System.Diagnostics.CodeAnalysis;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;
using Moq;

namespace EMS.Auth.API.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class LoggerMock
    {
        private static LogData LogData { get; set; }
        public static Mock<IEMSLogger<T>> SetupMock<T>(T _)
        {
            Mock<IEMSLogger<T>> mock = new();
            mock.Setup(m => m.AddLog(It.IsAny<LogData>())).Callback<LogData>((rro) => { LogData = rro; });
            mock.Setup(m => m.AddErrorLog(It.IsAny<LogData>())).Callback<LogData>((rro) => { LogData = rro; });

            return mock;
        }
    }
}
