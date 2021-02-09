using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Core.API.Models;
using EMS.Core.API.Services;
using EMS.Core.API.Tests.Mock;
using NUnit.Framework;

namespace EMS.Core.API.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class MotivationModificatorsServiceTest: BaseUnitTest<MotivationModificatorsService>
    {
        private MotivationModificator _motivationModificator1;
        private Staff _staff1;
        private Staff _staff2;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new MotivationModificatorsService(null, null, null));
            DbContextMock.ShouldThrowException = false;
            DbContextMock.SaveChangesResult = 1;

            _staff1 = new Staff
            {
                Id = 1
            };

            _staff2 = new Staff
            {
                Id = 2
            };
            _dbContext.Staff.Add(_staff1);
            _dbContext.Staff.Add(_staff2);

            _motivationModificator1 = new MotivationModificator
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                ModValue = 0.89,
                StaffId = _staff1.Id
            };

            _dbContext.MotivationModificators.Add(_motivationModificator1);

            _motivationModificatorRepository = new DAL.Repositories.MotivationModificatorRepository(_dbContext, _dateTimeUtil);
            _motivationModificatorsService = new MotivationModificatorsService(_motivationModificatorRepository, _logger, _dateTimeUtil);
        }
    }
}
