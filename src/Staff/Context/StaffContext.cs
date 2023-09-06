using EMS.Staff.Context.Configuration;
using EMS.Staff.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Staff.Context;

public sealed class StaffContext: DbContext
{
    public DbSet<Models.Staff> Staff { get; init; }
    public DbSet<History> History { get; init; }
    
    public StaffContext(DbContextOptions<StaffContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new HistoryConfiguration());
        modelBuilder.ApplyConfiguration(new StaffConfiguration());
    }
}