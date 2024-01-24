using EMS.Exceptions;
using EMS.Protos;
using EMS.Staff.Application.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;

namespace EMS.Staff.Infrastructure;

public sealed class PositionRepository: IPositionRepository
{
    private readonly PositionService.PositionServiceClient _positionServiceClient;

    public PositionRepository(PositionService.PositionServiceClient positionServiceClient)
    {
        _positionServiceClient = positionServiceClient;
    }

    public async Task ThrowExceptionIfPositionNotFoundAsync(int positionId, CancellationToken cancellationToken)
    {
        AsyncServerStreamingCall<Position>? call = _positionServiceClient.GetAll(new Empty(), cancellationToken: cancellationToken);
        List<Position>? positions = await call.ResponseStream.ToListAsync();
        if (positions.Exists(e => e.Grades.Any(g => g.ActualHistoryId == positionId)) is false)
            throw new NotFoundException($"Position with history id {positionId} does not exists");
    }
}