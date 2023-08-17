using EMS.Person.Context.Configuration;
using EMS.Person.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Person.Context;

public sealed class PersonContext: DbContext
{
    public DbSet<PersonInfo> People { get; init; }

    public PersonContext(DbContextOptions<PersonContext> options) : base(options) {}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        modelBuilder.ApplyConfiguration(new ContactConfiguration());
        modelBuilder.ApplyConfiguration(new ContactTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
    }
}