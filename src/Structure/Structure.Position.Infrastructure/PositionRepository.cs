using EMS.Exceptions;
using EMS.Structure.Position.Application.Interfaces;
using EMS.Structure.Position.Domain;
using EMS.Structure.Position.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EMS.Structure.Position.Infrastructure;

public sealed class PositionRepository: IPositionRepository
{
    private readonly PositionContext _context;

    public PositionRepository(PositionContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Domain.Position>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Positions
            .Include(e => e.Grades)
            .ThenInclude(e => e.History)
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<int> CreateAsync(string name, IDictionary<int, decimal> grades, CancellationToken cancellationToken)
    {
        if (await _context.Positions.AnyAsync(e => e.Name == name, cancellationToken))
            throw new AlreadyExistsException($"Position with name {name} already exists");

        Domain.Position position = new()
        {
            Name = name,
            Grades = MapGrades(grades)
        };
        
        await _context.Positions.AddAsync(position, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return position.Id;
    }

    public async Task UpdateSalaryAsync(int positionId,  KeyValuePair<int, decimal> gradeData, CancellationToken cancellationToken)
    {
        Grade? grade = await _context.Grades.FirstOrDefaultAsync(e => e.PositionId ==positionId && e.Value == gradeData.Key, cancellationToken);
        if (grade is null)
            throw new NotFoundException($"Grade {gradeData.Key} for position with id {positionId} not found");

        GradeHistory history = new()
        {
            GradeId = grade.Id,
            Value = gradeData.Value,
            CreatedOn = DateTime.UtcNow
        };

        await _context.GradeHistory.AddAsync(history, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private IEnumerable<Grade> MapGrades(IDictionary<int, decimal> grades)
    {
        return grades.Select(e => new Grade
        {
            Value = e.Key,
            History = new List<GradeHistory>
            {
                new()
                {
                    CreatedOn = DateTime.UtcNow,
                    Value = e.Value
                }
            }
        }).ToList();
    }
}