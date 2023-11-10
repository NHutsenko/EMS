using EMS.Structure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Structure.Infrastructure.Context.Configuration;

internal sealed class TeamConfiguration: IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Team", "dbo")
            .HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(128);

        builder.HasMany(e => e.Members)
            .WithOne(e => e.Team)
            .HasForeignKey(e => e.TeamId);
    }
}