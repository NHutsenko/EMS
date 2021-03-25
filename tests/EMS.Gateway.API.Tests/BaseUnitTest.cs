using System.Diagnostics.CodeAnalysis;
using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Gateway.API.Tests.Mock;
using Moq;
using static EMS.Common.Protos.DayOffs;
using static EMS.Common.Protos.Holidays;
using static EMS.Common.Protos.MotivationModificators;
using static EMS.Common.Protos.OtherPayments;
using static EMS.Common.Protos.Positions;
using static EMS.Common.Protos.Salary;
using static EMS.Common.Protos.Staffs;
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

        protected Mock<HolidaysClient> _holidaysClientMock;
        protected HolidaysClient _holidaysClient;

        protected Mock<DayOffsClient> _dayOffsClientMock;
        protected DayOffsClient _dayOffsClient;

        protected Mock<MotivationModificatorsClient> _motivationModificatorsClientMock;
        protected MotivationModificatorsClient _motivationModificatorsClient;

        protected Mock<OtherPaymentsClient> _otherPaymentsClientMock;
        protected OtherPaymentsClient _otherPaymentsClient;

        protected Mock<SalaryClient> _salaryClientMock;
        protected SalaryClient _salaryClient;

        protected Mock<StaffsClient> _staffsClientMock;
        protected StaffsClient _staffsClient;
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

            _holidaysClientMock = HolidaysClientMock.SetupMock();
            _holidaysClient = _holidaysClientMock.Object;

            _dayOffsClientMock = DayOffsClientMock.SetupMock();
            _dayOffsClient = _dayOffsClientMock.Object;

            _motivationModificatorsClientMock = MotivationModificatorsClientMock.SetupMock();
            _motivationModificatorsClient = _motivationModificatorsClientMock.Object;

            _otherPaymentsClientMock = OtherPaymentsClientMock.SetupMock();
            _otherPaymentsClient = _otherPaymentsClientMock.Object;

            _salaryClientMock = SalaryClientMock.SetupMock();
            _salaryClient = _salaryClientMock.Object;

            _staffsClientMock = StaffClientMock.SetupMock();
            _staffsClient = _staffsClientMock.Object;
        }

        public void InitializeLoggerMock(T loggerClass)
        {
            _loggerMock = LoggerMock.SetupMock(loggerClass);
            _logger = _loggerMock.Object;
        }
    }
}
