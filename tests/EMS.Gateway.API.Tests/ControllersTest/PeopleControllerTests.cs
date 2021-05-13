using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using EMS.Common.Logger.Models;
using EMS.Common.Protos;
using EMS.Gateway.API.Controllers;
using EMS.Gateway.API.Tests.Mock;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EMS.Gateway.API.Tests
{
    [ExcludeFromCodeCoverage]
    public class PeopleControllerTests: BaseUnitTest<PeopleController>
    {
        private PeopleController _peopleController;

        [SetUp]
        public void Setup()
        {
            InitializeMocks();
            InitializeLoggerMock(new PeopleController(null, null, null));
            _peopleController = new PeopleController(_peopleClient, _logger, _dateTimeUtil);
        }

        [Test]
        public void Add_should_return_response_from_grpc_client()
        {
            // Arrange
            PersonData request = new()
            {
                BornedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                LastName = "Test",
                Name = "Test",
                SecondName = "Test"
            };

            BaseResponse response = new()
            {
                Code = Code.Success,
                DataId = 1,
                ErrorMessage = string.Empty
            };

            BaseMock.Response = response;

            LogData expectedLog = new()
            {
                CallSide = nameof(PeopleController),
                CallerMethodName = nameof(_peopleController.Add),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _peopleController.Add(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _peopleClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void Add_should_handle_exception()
        {
            // Arrange
            PersonData request = new()
            {
                BornedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                LastName = "Test",
                Name = "Test",
                SecondName = "Test"
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(PeopleController),
                CallerMethodName = nameof(_peopleController.Add),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _peopleController.Add(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _peopleClientMock.Verify(m => m.AddAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddContact_should_return_response_from_grpc_client()
        {
            // Arrange
            ContactData request = new()
            {
               PersonId = 1,
               ContactType = 1,
               Name = "test",
               Value = "test"
            };

            BaseResponse response = new()
            {
                Code = Code.Success,
                DataId = 1,
                ErrorMessage = string.Empty
            };

            BaseMock.Response = response;

            LogData expectedLog = new()
            {
                CallSide = nameof(PeopleController),
                CallerMethodName = nameof(_peopleController.AddContact),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _peopleController.AddContact(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _peopleClientMock.Verify(m => m.AddContactAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddContact_should_handle_exception()
        {
            // Arrange
            ContactData request = new()
            {
                PersonId = 1,
                ContactType = 1,
                Name = "test",
                Value = "test"
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(PeopleController),
                CallerMethodName = nameof(_peopleController.AddContact),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _peopleController.AddContact(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _peopleClientMock.Verify(m => m.AddContactAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddPhoto_should_return_response_from_grpc_client()
        {
            // Arrange
            PhotoData request = new()
            {
               PersonId = 1,
               Name = "test.jpg",
               Base64 = "test",
               Mime = "image/jpg"
            };

            BaseResponse response = new()
            {
                Code = Code.Success,
                DataId = 1,
                ErrorMessage = string.Empty
            };

            BaseMock.Response = response;

            LogData expectedLog = new()
            {
                CallSide = nameof(PeopleController),
                CallerMethodName = nameof(_peopleController.AddPhoto),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _peopleController.AddPhoto(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _peopleClientMock.Verify(m => m.AddPhotoAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void AddPhoto_should_handle_exception()
        {
            // Arrange
            PhotoData request = new()
            {
                PersonId = 1,
                Name = "test.jpg",
                Base64 = "test",
                Mime = "image/jpg"
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(PeopleController),
                CallerMethodName = nameof(_peopleController.AddPhoto),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _peopleController.AddPhoto(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _peopleClientMock.Verify(m => m.AddPhotoAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void Update_should_return_response_from_grpc_client()
        {
            // Arrange
            PersonData request = new()
            {
                Id = 1,
                BornedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                LastName = "Test",
                Name = "Test",
                SecondName = "Test"
            };

            BaseResponse response = new()
            {
                Code = Code.Success,
                DataId = 1,
                ErrorMessage = string.Empty
            };

            BaseMock.Response = response;

            LogData expectedLog = new()
            {
                CallSide = nameof(PeopleController),
                CallerMethodName = nameof(_peopleController.Update),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _peopleController.Update(request) as ObjectResult;
            BaseResponse actualData = actual.Value as BaseResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _peopleClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void Update_should_handle_exception()
        {
            // Arrange
            PersonData request = new()
            {
                Id = 1,
                BornedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                LastName = "Test",
                Name = "Test",
                SecondName = "Test"
            };

            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(PeopleController),
                CallerMethodName = nameof(_peopleController.Update),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _peopleController.Update(request) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _peopleClientMock.Verify(m => m.UpdateAsync(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetAll_should_return_response_from_grpc_client()
        {
            // Arrange
            Empty request = new();

            PeopleResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            response.Data.Add(new PersonData
            {
                Id = 1,
                BornedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                LastName = "Test",
                Name = "Test",
                SecondName = "Test"
            });
            BaseMock.Response = response;

            LogData expectedLog = new()
            {
                CallSide = nameof(PeopleController),
                CallerMethodName = nameof(_peopleController.GetAll),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _peopleController.GetAll() as ObjectResult;
            PeopleResponse actualData = actual.Value as PeopleResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _peopleClientMock.Verify(m => m.GetAll(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetAll_should_handle_exception()
        {
            // Arrange
            Empty request = new();
            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(PeopleController),
                CallerMethodName = nameof(_peopleController.GetAll),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _peopleController.GetAll() as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _peopleClientMock.Verify(m => m.GetAll(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetById_should_return_response_from_grpc_client()
        {
            // Arrange
            ByPersonIdRequest request = new()
            {
                PersonId = 1
            };

            PersonResponse response = new()
            {
                Status = new BaseResponse
                {
                    Code = Code.Success,
                    ErrorMessage = string.Empty
                }
            };
            response.Data = new PersonData
            {
                Id = 1,
                BornedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                CreatedOn = Timestamp.FromDateTime(_dateTimeUtil.GetCurrentDateTime()),
                LastName = "Test",
                Name = "Test",
                SecondName = "Test"
            };
            BaseMock.Response = response;

            LogData expectedLog = new()
            {
                CallSide = nameof(PeopleController),
                CallerMethodName = nameof(_peopleController.GetById),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = response
            };

            // Act
            ObjectResult actual = _peopleController.GetById(request.PersonId) as ObjectResult;
            PersonResponse actualData = actual.Value as PersonResponse;

            // Assert
            Assert.AreEqual(200, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(response, actualData, "Response data as expected");
            _peopleClientMock.Verify(m => m.GetById(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddLog(expectedLog), Times.Once);
        }

        [Test]
        public void GetById_should_handle_exception()
        {
            // Arrange
            ByPersonIdRequest request = new()
            {
                PersonId = 1
            };
            BaseMock.ShouldThrowException = true;

            LogData expectedLog = new()
            {
                CallSide = nameof(PeopleController),
                CallerMethodName = nameof(_peopleController.GetById),
                CreatedOn = _dateTimeUtil.GetCurrentDateTime(),
                Request = request,
                Response = new Exception(BaseMock.ExceptionMessage)
            };

            // Act
            ObjectResult actual = _peopleController.GetById(request.PersonId) as ObjectResult;

            // Assert
            Assert.AreEqual(500, actual.StatusCode, "StatusCode as expected");
            Assert.AreEqual(BaseMock.ErrorResponseMessage, actual.Value, "Response data as expected");
            _peopleClientMock.Verify(m => m.GetById(request, null, null, new CancellationToken()), Times.Once);
            _loggerMock.Verify(m => m.AddErrorLog(expectedLog), Times.Once);
        }
    }
}
