using EMS.Staff.Context.Configuration;
using EMS.Staff.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Staff.Context;

public sealed class StaffContext: DbContext
{
    public DbSet<Grade> Grades { get; init; }
    public DbSet<GradeHistory> GradeHistory { get; init; }
    public DbSet<Position> Positions { get; init; }

    public StaffContext(DbContextOptions<StaffContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GradeConfiguration());
        modelBuilder.ApplyConfiguration(new GradeHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new PositionConfiguration());
    }
}