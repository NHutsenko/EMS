using System.Diagnostics.CodeAnalysis;
using EMS.Person.Domain;
using EMS.Protos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Person.Infrastructure.Context.Configuration;

[ExcludeFromCodeCoverage]
internal sealed class ContactTypeConfiguration : IEntityTypeConfiguration<ContactType>
{
    public void Configure(EntityTypeBuilder<ContactType> builder)
    {
        builder.ToTable("ContactType", "dbo")
            .HasKey(e => e.Id);

        builder.Property(e => e.Value)
            .HasMaxLength(128);

        builder.HasData(GetEntities());
    }

    private static ContactType[] GetEntities()
    {
        return Enum.GetValues<ContactData.Types.ContactType>()
            .Where(e => e != ContactData.Types.ContactType.Undefined)
            .Select(e => new ContactType
            {
                Id = (int)e,
                Value = e.ToString()
            })
            .ToArray();
    }
}