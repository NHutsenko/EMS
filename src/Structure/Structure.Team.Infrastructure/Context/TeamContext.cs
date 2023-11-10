using EMS.Structure.Team.Domain;
using EMS.Structure.Team.Infrastructure.Context.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EMS.Structure.Team.Infrastructure.Context;

public sealed class TeamContext: DbContext
{
   
    public DbSet<Domain.Team> Teams { get; init; }
    public DbSet<Member> Members { get; init; }

    public TeamContext(DbContextOptions<TeamContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TeamConfiguration());
        modelBuilder.ApplyConfiguration(new MemberConfiguration());
    }
}