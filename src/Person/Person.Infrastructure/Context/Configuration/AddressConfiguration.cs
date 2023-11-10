using System.Diagnostics.CodeAnalysis;
using EMS.Person.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Person.Infrastructure.Context.Configuration;

[ExcludeFromCodeCoverage]
internal sealed class AddressConfiguration: IEntityTypeConfiguration<Address>
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