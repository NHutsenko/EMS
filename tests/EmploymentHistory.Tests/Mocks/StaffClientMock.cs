using System.Diagnostics.CodeAnalysis;
using EMS.Protos;
using Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace EMS.EmploymentHistory.Tests.Mocks;

[ExcludeFromCodeCoverage]
internal sealed class StaffClientMock
{
    public StaffService.StaffServiceClient StaffClient { get; init; }
    public StaffData PersonStaffHistoryResponse { get; init; }
    public Int32Value PersonStaffFoundRequest { get; init; }
    public Int32Value PersonStaffNotFoundRequest { get; init; }

    public StaffClientMock()
    {
        StaffClient = Substitute.For<StaffService.StaffServiceClient>(GrpcCoreMock.Channel);

        Staff staffOne = new()
        {
            Id = 1,
            Manager = 2,
            Position = 1,
            History = new StaffHistory
            {
                Person = 1,
                Mentor = 3,
                CreatedOn = new DateTime(2023, 5, 1, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
                Employment = 100
            }
        };
        Staff staffTwo = new()
        {
            Id = 2,
            Manager = 2,
            Position = 2,
            History = new StaffHistory
            {
                Person = 1,
                Mentor = 3,
                CreatedOn = new DateTime(2023, 10, 1, 0, 0, 0, DateTimeKind.Utc).ToTimestamp(),
                Employment = 100
            }
        };

        PersonStaffHistoryResponse = new StaffData
        {
            Data = { staffOne, staffTwo }
        };

        PersonStaffFoundRequest = new Int32Value
        {
            Value = 1
        };
        PersonStaffNotFoundRequest = new Int32Value
        {
            Value = 999
        };

        StaffClient.GetByPersonAsync(PersonStaffFoundRequest, Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetAsyncUnaryCallResponse(PersonStaffHistoryResponse));
        StaffClient.GetByPersonAsync(PersonStaffNotFoundRequest, Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Throws(new NotFoundException($"Staff for person with id {PersonStaffNotFoundRequest.Value} not found").ToRpcException());
    }
}