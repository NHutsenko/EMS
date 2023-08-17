using System.Diagnostics.CodeAnalysis;
using EMS.Person.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Person.Context.Configuration;

[ExcludeFromCodeCoverage]
public sealed class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contact", "dbo")
            .HasKey(e => e.Id);

        builder.Property(e => e.Value)
            .HasMaxLength(128);

        builder.HasOne(e => e.ContactType)
            .WithMany(e => e.Contacts)
            .HasForeignKey(e => e.ContactTypeId);
    }
}