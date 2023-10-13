using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using EMS.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;

namespace EMS.EmploymentHistory.Tests.Mocks;

[ExcludeFromCodeCoverage]
internal sealed class PositionClientMock
{
    public PositionService.PositionServiceClient PositionClient { get; init; }
    public Position PositionResponse { get; init; }
    public Position PositionTwo { get; init; }

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
        
        PositionTwo = new Position
        {
            Id = 2,
            Name = "Test2",
            Grades =
            {
                new List<Grade>
                {
                    new()
                    {
                        Value = 2,
                        ActualHistoryId = 2,
                        Salary = new DecimalValue
                        {
                            Units = 2000
                        }
                    }
                }
            }
        };

        List<Position> positionsResponse = new()
        {
            PositionResponse, PositionTwo
        };
        
        PositionClient.GetAll(Arg.Any<Empty>(), Arg.Any<Metadata>(), Arg.Any<DateTime?>(), Arg.Any<CancellationToken>())
            .Returns(GrpcCoreMock.GetStreamResponse(positionsResponse));
    }
}