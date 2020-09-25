using EMS.Gateway.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Gateway.API.DAL
{
	public class ApplicationDbContext: DbContext, IApplicationDbContext
	{
		public DbSet<Position> Positions { get; set; }
		public DbSet<Team> Teams { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Position>()
				.ToTable("Positions", "core")
				.HasOne(e => e.Team)
				.WithMany(e => e.Positions)
				.HasForeignKey(e => e.TeamId);
			modelBuilder.Entity<Team>()
				.ToTable("Teams", "core");
		}
	}
}
