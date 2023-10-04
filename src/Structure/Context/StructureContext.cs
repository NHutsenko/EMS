using EMS.Structure.Context.Configuration;
using EMS.Structure.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Structure.Context;

public sealed class StructureContext: DbContext
{
    public DbSet<Grade> Grades { get; init; }
    public DbSet<GradeHistory> GradeHistory { get; init; }
    public DbSet<Position> Positions { get; init; }
    
    public DbSet<Team> Teams { get; init; }
    public DbSet<Member> Members { get; init; }

    public StructureContext(DbContextOptions<StructureContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GradeConfiguration());
        modelBuilder.ApplyConfiguration(new GradeHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new PositionConfiguration());

        modelBuilder.ApplyConfiguration(new TeamConfiguration());
        modelBuilder.ApplyConfiguration(new MemberConfiguration());

        Position endWorkPosition = new Position
        {
            Id = -1,
            Name = "End Work",
            Grades = new List<Grade>
            {
                new()
                {
                    Id = -1,
                    Value = 0,
                    History = new List<GradeHistory>
                    {
                        new()
                        {
                            Id = -1,
                            CreatedOn = DateTime.UtcNow,
                            Value = 0
                        }
                    }
                }
            }
        };
        modelBuilder.Entity<Position>()
            .HasData(endWorkPosition);
    }
}