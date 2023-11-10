using System.Diagnostics.CodeAnalysis;
using EMS.Person.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Person.Infrastructure.Context.Configuration;

[ExcludeFromCodeCoverage]
internal sealed class PersonConfiguration : IEntityTypeConfiguration<PersonInfo>
{
    public void Configure(EntityTypeBuilder<PersonInfo> builder)
    {
        builder.ToTable("Person", "dbo")
            .HasKey(e => e.Id);

        builder.Property(e => e.Login)
            .HasMaxLength(128);
        builder.Property(e => e.LastName)
            .HasMaxLength(128);
        builder.Property(e => e.FirstName)
            .HasMaxLength(128);
        builder.Property(e => e.About)
            .HasMaxLength(1024);

        builder.HasOne(e => e.Address)
            .WithOne(e => e.Person)
            .HasForeignKey<Address>(e => e.PersonId);

        builder.HasMany(e => e.Contacts)
            .WithOne(e => e.Person)
            .HasForeignKey(e => e.PersonId);
    }
}