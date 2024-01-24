using System.Diagnostics.CodeAnalysis;
using Grpc.Core;
using NSubstitute;

namespace EMS.EmploymentHistory.Tests.Mocks;

[ExcludeFromCodeCoverage]
internal sealed class StaffClientMock
{
    public StaffService.StaffServiceClient StaffClient { get; init; }
    public List<Staff> PersonStaffHistoryResponse { get; init; }
    public Staff StaffFirst { get; init; }
    public Staff StaffSecond { get; init; }
    public Int32Value PersonStaffFoundRequest { get; init; }
    public Int32Value StaffFoundRequest { get; init; }
    public Int32Value StaffCreateResponse { get; init; }

    public StaffClientMock()
    {
        StaffClient = Substitute.For<StaffService.StaffServiceClient>(GrpcCoreMock.Channel);

        StaffFirst = new()
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
        StaffSecond = new()
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
            StaffFirst, StaffSecond
        };

        PersonStaffFoundRequest = new Int32Value
        {
            Value = 1
        };
        StaffClient.GetByPerson(PersonStaffFoundRequest, Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetStreamResponse(PersonStaffHistoryResponse));

        StaffCreateResponse = new Int32Value
        {
            Value = PersonStaffHistoryResponse.Count + 1
        };
        StaffClient.CreateAsync(Arg.Any<NewStaff>(), Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetAsyncUnaryCallResponse(StaffCreateResponse));
        StaffClient.CreateHistoryAsync(Arg.Any<NewHistory>(), Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetAsyncUnaryCallResponse(new Empty()));

        StaffFoundRequest = new Int32Value
        {
            Value = StaffSecond.Id
        };
        StaffClient.GetByHistoryIdAsync(StaffFoundRequest, Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetAsyncUnaryCallResponse(StaffSecond));
        
        StaffClient.SetManagerAsync(Arg.Any<NewManager>(),Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetAsyncUnaryCallResponse(new Empty()));
        
        StaffClient.SetPositionAsync(Arg.Any<NewPosition>(),Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetAsyncUnaryCallResponse(new Empty()));
        
        StaffClient.SetDateAsync(Arg.Any<NewDate>(),Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetAsyncUnaryCallResponse(new Empty()));
        
        StaffClient.SetEmploymentAsync(Arg.Any<NewEmployment>(),Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetAsyncUnaryCallResponse(new Empty()));
        
        StaffClient.SetMentorAsync(Arg.Any<NewMentor>(),Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetAsyncUnaryCallResponse(new Empty()));
    }
}