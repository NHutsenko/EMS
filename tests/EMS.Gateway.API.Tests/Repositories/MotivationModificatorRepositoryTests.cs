using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.Models;
using EMS.Core.API.Tests.Mock;
using Moq;
using NUnit.Framework;

namespace EMS.Core.API.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class MotivationModificatorRepositoryTests : BaseUnitTest<MotivationModificatorRepository>
    {
        private MotivationModificator _motivationModificator1;
        private MotivationModificator _motivationModificator2;
        private Staff _staff1;
        private Staff _staff2;
        private Staff _staff3;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            DbContextMock.ShouldThrowException = false;

            _staff1 = new Staff
            {
                Id = 1
            };
            _staff2 = new Staff
            {
                Id = 2
            };
            _staff3 = new Staff
            {
                Id = 3
            };
            _dbContext.Staff.Add(_staff1);
            _dbContext.Staff.Add(_staff2);
            _dbContext.Staff.Add(_staff3);

            _motivationModificator1 = new MotivationModificator
            {
                Id = 1,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                ModValue = 0.8,
                StaffId = _staff1.Id
            };
            _motivationModificator2 = new MotivationModificator
            {
                Id = 2,
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                ModValue = 0.9,
                StaffId = _staff2.Id
            };
            _dbContext.MotivationModificators.Add(_motivationModificator1);
            _dbContext.MotivationModificators.Add(_motivationModificator2);
            _motivationModificatorRepository = new MotivationModificatorRepository(_dbContext, _dateTimeUtil);
        }

        [Test]
        public void GetAll_should_return_all_modificators_from_db()
        {
            // Arrange
            List<MotivationModificator> expected = new List<MotivationModificator>
            {
                _motivationModificator1, _motivationModificator2
            };

            // Act
            IQueryable<MotivationModificator> actual = _motivationModificatorRepository.GetAll();

            // Assert
            CollectionAssert.AreEqual(expected, actual, "Data returnted as expected");
        }

        [Test]
        public void GetByStaffId_should_return_modificator_with_specified_staffId()
        {
            // Act
            MotivationModificator actual = _motivationModificatorRepository.GetByStaffId(_motivationModificator1.StaffId);

            // Assert
            Assert.AreEqual(_motivationModificator1, actual, "Data returnted as expected");
        }

        [Test]
        public void AddAsync_should_add_extity_to_db()
        {
            // Arrange
            MotivationModificator motivationModificator = new MotivationModificator
            {
                ModValue = 0.5,
                StaffId = _staff3.Id
            };

            // Act
            int result = _motivationModificatorRepository.AddAsync(motivationModificator).Result;
            MotivationModificator actual = _dbContext.MotivationModificators.FirstOrDefault(e => e.Id == motivationModificator.Id); 
            // Assert
            Assert.AreEqual(1, result, "Added as expected");
            Assert.AreEqual(motivationModificator, actual, "Data in modificator as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void AddAsync_should_throw_null_reference_exception_because_entity_is_null()
        {
            // Arrange
            MotivationModificator motivationModificator = new MotivationModificator
            {
                ModValue = 0.5,
                StaffId = _staff3.Id
            };

            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _motivationModificatorRepository.AddAsync(null), "Throws NullReferenceException as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_argument_exception_because_staffId_does_not_exists_in_db()
        {
            // Arrange
            MotivationModificator motivationModificator = new MotivationModificator
            {
                ModValue = 0.5,
                StaffId = 4
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _motivationModificatorRepository.AddAsync(motivationModificator), "Throws ArgumentException as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }


        [Test]
        public void AddAsync_should_argument_exception_because_staffId_is_equal_to_zero()
        {
            // Arrange
            MotivationModificator motivationModificator = new MotivationModificator
            {
                ModValue = 0.5,
                StaffId = 0
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _motivationModificatorRepository.AddAsync(motivationModificator), "Throws ArgumentException as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void AddAsync_should_argument_exception_because_motivation_modificator_is_wrong()
        {
            // Arrange
            MotivationModificator motivationModificator = new MotivationModificator
            {
                ModValue = -0.5,
                StaffId = _staff1.Id
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _motivationModificatorRepository.AddAsync(motivationModificator), "Throws ArgumentException as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_add_extity_to_db()
        {
            // Arrange
            MotivationModificator motivationModificator = new MotivationModificator
            {
                Id = _motivationModificator1.Id,
                ModValue = 0.5,
                StaffId = _staff3.Id
            };

            // Act
            int result = _motivationModificatorRepository.UpdateAsync(motivationModificator).Result;
            MotivationModificator actual = _dbContext.MotivationModificators.FirstOrDefault(e => e.Id == motivationModificator.Id);
            // Assert
            Assert.AreEqual(1, result, "Updated as expected");
            Assert.AreEqual(motivationModificator, actual, "Data in modificator as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Once);
        }

        [Test]
        public void UpdateAsync_should_throw_null_reference_exception_because_entity_is_null()
        {
            // Arrange
            MotivationModificator motivationModificator = new MotivationModificator
            {
                ModValue = 0.5,
                StaffId = _staff3.Id
            };

            // Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _motivationModificatorRepository.UpdateAsync(null), "Throws NullReferenceException as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_argument_exception_because_staffId_does_not_exists_in_db()
        {
            // Arrange
            MotivationModificator motivationModificator = new MotivationModificator
            {
                ModValue = 0.5,
                StaffId = 4,
                Id = _motivationModificator1.Id
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _motivationModificatorRepository.UpdateAsync(motivationModificator), "Throws ArgumentException as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }


        [Test]
        public void UpdateAsync_should_argument_exception_because_staffId_is_equal_to_zero()
        {
            // Arrange
            MotivationModificator motivationModificator = new MotivationModificator
            {
                ModValue = 0.5,
                StaffId = 0,
                Id = _motivationModificator1.Id
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _motivationModificatorRepository.UpdateAsync(motivationModificator), "Throws ArgumentException as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }

        [Test]
        public void UpdateAsync_should_argument_exception_because_motivation_modificator_is_wrong()
        {
            // Arrange
            MotivationModificator motivationModificator = new MotivationModificator
            {
                ModValue = -0.5,
                StaffId = _staff1.Id,
                Id = _motivationModificator1.Id
            };

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _motivationModificatorRepository.UpdateAsync(motivationModificator), "Throws ArgumentException as expected");
            _dbContextMock.Verify(m => m.SaveChangesAsync(true, new CancellationToken()), Times.Never);
        }
    }
}
