using System.Diagnostics.CodeAnalysis;
using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Gateway.API.Tests.Mock;
using Moq;
using static EMS.Common.Protos.Positions;
using static EMS.Common.Protos.Teams;

namespace EMS.Gateway.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class BaseUnitTest<T>
    {
        // Grpc clients
        protected Mock<TeamsClient> _teamsClientMock;
        protected TeamsClient _teamsClient;

        protected Mock<PositionsClient> _positionsClientMock;
        protected PositionsClient _positionsClient;

        // Logger
        protected Mock<IEMSLogger<T>> _loggerMock;
        protected IEMSLogger<T> _logger;

        // Utils
        protected IDateTimeUtil _dateTimeUtil;

        public void InitializeMocks()
        {
            BaseMock.Response = null;
            BaseMock.ShouldThrowException = false;

            _dateTimeUtil = new DateTimeUtilMock();

            _teamsClientMock = TeamsClientMock.SetupMock();
            _teamsClient = _teamsClientMock.Object;

            _positionsClientMock = PositionsClientMock.SetupMock();
            _positionsClient = _positionsClientMock.Object;
        }

        public void InitializeLoggerMock(T loggerClass)
        {
            _loggerMock = LoggerMock.SetupMock(loggerClass);
            _logger = _loggerMock.Object;
        }
    }
}
