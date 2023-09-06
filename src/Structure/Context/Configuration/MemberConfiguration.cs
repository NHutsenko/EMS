using EMS.Structure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Structure.Context.Configuration;

public sealed class MemberConfiguration: IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("TeamMembers", "dbo")
            .HasKey(e => e.Id);
    }
}