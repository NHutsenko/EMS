using EMS.Structure.Team.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Structure.Team.Infrastructure.Context.Configuration;

internal sealed class MemberConfiguration: IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("TeamMembers", "dbo")
            .HasKey(e => e.Id);
    }
}