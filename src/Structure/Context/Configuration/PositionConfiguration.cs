using EMS.Staff.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Staff.Context.Configuration;

public class PositionConfiguration: IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
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