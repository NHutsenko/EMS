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
    public List<Staff> PersonStaffHistoryResponse { get; init; }
    public Int32Value PersonStaffFoundRequest { get; init; }
    public Int32Value PersonStaffNotFoundRequest { get; init; }
    
    public Int32Value StaffCreateResponse { get; init; }

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

        PersonStaffHistoryResponse = new List<Staff>
        {
            staffOne, staffTwo
        };

        PersonStaffFoundRequest = new Int32Value
        {
            Value = 1
        };
        StaffClient.GetByPerson(PersonStaffFoundRequest, Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetStreamResponse(PersonStaffHistoryResponse));
        
        PersonStaffNotFoundRequest = new Int32Value
        {
            Value = 999
        };
        StaffClient.GetByPerson(PersonStaffNotFoundRequest, Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Throws(new NotFoundException($"Staff for person with id {PersonStaffNotFoundRequest.Value} not found").ToRpcException());

        StaffCreateResponse = new Int32Value
        {
            Value = PersonStaffHistoryResponse.Count + 1
        };
        StaffClient.CreateAsync(Arg.Any<NewStaff>(), Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetAsyncUnaryCallResponse(StaffCreateResponse));
        StaffClient.CreateHistoryAsync(Arg.Any<NewHistory>(), Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetAsyncUnaryCallResponse(new Empty()));
    }
}