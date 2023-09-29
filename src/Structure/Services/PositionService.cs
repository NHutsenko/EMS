using EMS.Exceptions;
using EMS.Protos;
using EMS.Structure.Context;
using Exceptions;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using GradeHistory = EMS.Structure.Models.GradeHistory;

namespace EMS.Structure.Services;

public sealed class PositionService : Protos.PositionService.PositionServiceBase
{
    private readonly StructureContext _dbContext;

    public PositionService(StructureContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<PositionsReply> GetAll(Empty request, ServerCallContext context)
    {
        IEnumerable<Position> data = (await _dbContext.Positions
                .Include(e => e.Grades)
                .ThenInclude(e => e.History)
                .AsNoTracking()
                .ToListAsync())
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

        PositionsReply reply = new()
        {
            Data = { data }
        };
        return reply;
    }

    public override async Task<Int32Value> Create(PositionRequest request, ServerCallContext context)
    {
        if (await _dbContext.Positions.AnyAsync(e => e.Name == request.Name, context.CancellationToken))
        {
            throw new AlreadyExistsException($"Position with name {request.Name} already exists");
        }

        Models.Position position = new()
        {
            Name = request.Name,
            Grades = MapGrades(request.Grades)
        };
        
        await _dbContext.Positions.AddAsync(position, context.CancellationToken);
        await _dbContext.SaveChangesAsync(context.CancellationToken);
        
        return new Int32Value
        {
            Value = position.Id
        };
    }

    public override async Task<Empty> UpdateGradeSalary(NewGradeSalaryRequest request, ServerCallContext context)
    {
        Models.Grade? grade = await _dbContext.Grades.FirstOrDefaultAsync(e => e.PositionId == request.PositionId && e.Value == request.Grade, context.CancellationToken);
        if (grade is null)
        {
            throw new NotFoundException($"Grade {request.Grade} for position with id {request.PositionId} not found");
        }

        GradeHistory history = new()
        {
            GradeId = grade.Id,
            Value = request.Salary,
            CreatedOn = DateTime.UtcNow
        };

        await _dbContext.GradeHistory.AddAsync(history, context.CancellationToken);
        await _dbContext.SaveChangesAsync(context.CancellationToken);
        
        return new Empty();
    }

    private GradeHistory GetLastGrade(IEnumerable<GradeHistory> history)
    {
        return history.Where(h => h.CreatedOn < DateTime.Now)
            .OrderByDescending(h => h.CreatedOn)
            .First();
    }

    private IEnumerable<Models.Grade> MapGrades(RepeatedField<Grade> grades)
    {
        return grades.Select(e => new Models.Grade
            {
                Value = e.Value,
                History = new List<GradeHistory>
                {
                    new()
                    {
                        CreatedOn = DateTime.UtcNow,
                        Value = e.Salary
                    }
                }
            })
            .ToList();
    }
}