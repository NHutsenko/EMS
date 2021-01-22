using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Moq;
using LoggerExtensions;
using EMS.Common.Models.BaseModel;

namespace EMS.Core.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class LoggerMock
    {
        public static Mock<ILogger<T>> SetupMock<T>(T loggerClass)
        {
            Mock<ILogger<T>> mock = new Mock<ILogger<T>>();

            mock.Setup(m => m.AddLog(It.IsAny<RequestResponseObject>())).Callback<RequestResponseObject>((rro) => { });
            mock.Setup(m => m.AddErrorLog(It.IsAny<RequestResponseObject>())).Callback<RequestResponseObject>((rro) => { });

            return mock;
        }
    }
}
