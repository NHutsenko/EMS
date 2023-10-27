using EMS.Exceptions;
using EMS.Protos;
using EMS.Structure.Context;
using EMS.Structure.Interfaces;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;
using GradeHistory = EMS.Structure.Models.GradeHistory;

namespace EMS.Structure.Services;

public sealed class PositionService : Protos.PositionService.PositionServiceBase
{
    private readonly IPositionRepository _positionRepository;

    public PositionService(StructureContext dbContext, IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }

    public override async Task GetAll(Empty request, IServerStreamWriter<Position> responseStream, ServerCallContext context)
    {
        IEnumerable<Position> data = (await _positionRepository.GetAllAsync(context.CancellationToken))
            .Select(e => new Position
            {
                Id = e.Id,
                Name = e.Name,
                Grades =
                {
                    e.Grades.Select(g => new Grade
                    {
                        Value = g.Value,
                        ActualHistoryId = GetLastGrade(g.History).Id,
                        Salary = GetLastGrade(g.History).Value
                    })
                }
            });

        await responseStream.WriteAllAsync(data);
    }

    public override async Task<Int32Value> Create(PositionRequest request, ServerCallContext context)
    {
        IDictionary<int, decimal> grades = request.Grades.ToDictionary(e => e.Value, e => e.Salary.ToDecimal());
        int id = await _positionRepository.CreateAsync(request.Name, grades, context.CancellationToken);
        
        return new Int32Value
        {
            Value = id
        };
    }

    public override async Task<Empty> UpdateGradeSalary(NewGradeSalaryRequest request, ServerCallContext context)
    {
        KeyValuePair<int, decimal> gradeData = new (request.Grade, request.Salary.ToDecimal());
        await _positionRepository.UpdateSalaryAsync(request.PositionId, gradeData, context.CancellationToken);
        
        return new Empty();
    }

    private GradeHistory GetLastGrade(IEnumerable<GradeHistory> history)
    {
        return history.Where(h => h.CreatedOn < DateTime.Now)
            .OrderByDescending(h => h.CreatedOn)
            .First();
    }
}