using EMS.Staff.Domain;
using EMS.Staff.Infrastructure.Context.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EMS.Staff.Infrastructure.Context;

public sealed class StaffContext: DbContext
{
    public DbSet<Domain.Staff> Staff { get; init; }
    public DbSet<History> History { get; init; }
    
    public StaffContext(DbContextOptions<StaffContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new HistoryConfiguration());
        modelBuilder.ApplyConfiguration(new StaffConfiguration());
    }
}