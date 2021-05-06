using System.Diagnostics.CodeAnalysis;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Auth.API.DAL
{
    [ExcludeFromCodeCoverage]
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> Tokens { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("Users", "auth")
                .HasIndex(e => e.Login)
                .IsUnique();
            modelBuilder.Entity<UserToken>()
                .ToTable("Tokens", "auth")
                .HasOne(e => e.User)
                .WithMany(e => e.Tokens)
                .HasForeignKey(e => e.UserId);
            modelBuilder.Entity<UserToken>()
                .HasIndex(e => new { e.AccessToken, e.RefreshToken });
        }
    }
}
