using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using EMS.Person.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Person.Context.Configuration;

[ExcludeFromCodeCoverage]
public sealed class AddressConfiguration: IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Address", "dbo")
            .HasKey(e => e.Id);

        builder.Property(e => e.City)
            .HasMaxLength(128);

        builder.Property(e => e.Street)
            .HasMaxLength(128);

        builder.Property(e => e.Building)
            .HasMaxLength(8);
        builder.Property(e => e.House)
            .HasMaxLength(8);
    }
}