using EMS.Auth.API.DAL.Interfaces;
using EMS.Auth.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Auth.API.DAL
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Login)
                .IsUnique();
        }
    }
}
