using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Structure.Position.Infrastructure.Context.Configuration;

internal sealed class PositionConfiguration: IEntityTypeConfiguration<Domain.Position>
{
    public void Configure(EntityTypeBuilder<Domain.Position> builder)
    {
        builder.ToTable("Position", "dbo")
            .HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(128);

        builder.HasMany(e => e.Grades)
            .WithOne(e => e.Position)
            .HasForeignKey(e => e.PositionId);
    }
}