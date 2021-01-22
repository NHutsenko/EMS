using System.Diagnostics.CodeAnalysis;
using Moq;
using EMS.Common.Models.BaseModel;
using EMS.Common.Logger;

namespace EMS.Core.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class LoggerMock
    {
        private static RequestResponseObject LogData { get; set; }
        public static Mock<IEMSLogger<T>> SetupMock<T>(T _)
        {
            Mock<IEMSLogger<T>> mock = new Mock<IEMSLogger<T>>();
            mock.Setup(m => m.AddLog(It.IsAny<RequestResponseObject>())).Callback<RequestResponseObject>((rro) => { LogData = rro; });
            mock.Setup(m => m.AddErrorLog(It.IsAny<RequestResponseObject>())).Callback<RequestResponseObject>((rro) => { LogData = rro; });

            return mock;
        }
    }
}
