using System.Diagnostics.CodeAnalysis;
using Moq;
using EMS.Common.Logger;
using EMS.Common.Logger.Models;

namespace EMS.Core.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class LoggerMock
    {
        private static LogData LogData { get; set; }
        public static Mock<IEMSLogger<T>> SetupMock<T>(T _)
        {
            Mock<IEMSLogger<T>> mock = new Mock<IEMSLogger<T>>();
            mock.Setup(m => m.AddLog(It.IsAny<LogData>())).Callback<LogData>((rro) => { LogData = rro; });
            mock.Setup(m => m.AddErrorLog(It.IsAny<LogData>())).Callback<LogData>((rro) => { LogData = rro; });

            return mock;
        }
    }
}
