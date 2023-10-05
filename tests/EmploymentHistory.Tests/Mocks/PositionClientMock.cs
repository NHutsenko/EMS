using System.Diagnostics.CodeAnalysis;
using EMS.Protos;
using Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace EMS.EmploymentHistory.Tests.Mocks;

[ExcludeFromCodeCoverage]
internal sealed class PositionClientMock
{
    public PositionService.PositionServiceClient PositionClient { get; init; }
    public Position PositionResponse { get; init; }

    public PositionClientMock()
    {
        PositionClient = Substitute.For<PositionService.PositionServiceClient>(GrpcCoreMock.Channel);

        PositionResponse = new Position
        {
            Id = 1,
            Name = "Test",
            Grades =
            {
                new List<Grade>
                {
                    new()
                    {
                        Value = 1,
                        ActualHistoryId = 1,
                        Salary = new DecimalValue
                        {
                            Units = 1000
                        }
                    }
                }
            }
        };

        List<Position> positionsResponse = new()
        {
            PositionResponse
        };
        
        PositionClient.GetAll(Arg.Any<Empty>(), Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetStreamResponse(positionsResponse));
    }
}