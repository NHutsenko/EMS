using EMS.Structure.Position.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Structure.Position.Infrastructure.Context.Configuration;

internal sealed class GradeConfiguration: IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.ToTable("PositionGrade", "dbo")
            .HasKey(e => e.Id);

        builder.HasMany(e => e.History)
            .WithOne(e => e.Grade)
            .HasForeignKey(e => e.GradeId);
    }
}