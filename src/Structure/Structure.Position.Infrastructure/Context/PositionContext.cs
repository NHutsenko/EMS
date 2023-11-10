using EMS.Structure.Position.Domain;
using EMS.Structure.Position.Infrastructure.Context.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EMS.Structure.Position.Infrastructure.Context;

public sealed class PositionContext: DbContext
{
    public DbSet<Grade> Grades { get; init; }
    public DbSet<GradeHistory> GradeHistory { get; init; }
    public DbSet<Domain.Position> Positions { get; init; }

    public PositionContext(DbContextOptions<PositionContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GradeConfiguration());
        modelBuilder.ApplyConfiguration(new GradeHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new PositionConfiguration());

        
        Domain.Position endWorkPosition = new Domain.Position
        {
            Id = -1,
            Name = "End Work"
        };
        Grade grade = new()
        {
            Id = -1,
            PositionId = -1,
            Value = 0
        };
        GradeHistory gradeHistory = new()
        {
            Id = -1,
            GradeId = -1,
            Value = 0
        };
        
        modelBuilder.Entity<Domain.Position>()
            .HasData(endWorkPosition);
        modelBuilder.Entity<Grade>()
            .HasData(grade);
        modelBuilder.Entity<GradeHistory>()
            .HasData(gradeHistory);
    }
}