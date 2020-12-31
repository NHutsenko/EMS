using System.Diagnostics.CodeAnalysis;
using EMS.Core.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Core.API.DAL
{
    [ExcludeFromCodeCoverage]
	public class ApplicationDbContext: DbContext, IApplicationDbContext
	{
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<DayOff> DaysOff { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<OtherPayment> OtherPayments { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PersonPhoto> Photos { get; set; }
		public DbSet<Position> Positions { get; set; }
        public DbSet<Staff> Staff { get; set; }
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
            modelBuilder.Entity<Staff>()
                .ToTable("Staff", "core")
                .HasOne(e => e.Position)
                .WithMany(e => e.Staff)
                .HasForeignKey(e => e.PositionId);

            modelBuilder.Entity<Person>()
                .ToTable("People", "core")
                .HasMany(e => e.Staff)
                .WithOne(e => e.Person)
                .HasForeignKey(e => e.PersonId);

            modelBuilder.Entity<DayOff>()
                .ToTable("DayOffs", "core")
                .HasOne(e => e.Staff)
                .WithMany(e => e.DayOff)
                .HasForeignKey(e => e.StaffId);

            modelBuilder.Entity<Contact>()
                .ToTable("Contacts", "core")
                .HasOne(e => e.Person)
                .WithMany(e => e.Contacts)
                .HasForeignKey(e => e.PersonId);

            modelBuilder.Entity<PersonPhoto>()
                .ToTable("PersonPhotos", "core")
                .HasOne(e => e.Person)
                .WithMany(e => e.Photos)
                .HasForeignKey(e => e.PersonId);

            modelBuilder.Entity<OtherPayment>()
                .ToTable("OtherPayments", "core")
                .HasOne(e => e.Person)
                .WithMany(e => e.OtherPayments)
                .HasForeignKey(e => e.PersonId);

            modelBuilder.Entity<Holiday>()
                .ToTable("Holidays", "core");
        }
	}
}
