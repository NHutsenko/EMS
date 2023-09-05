using EMS.Staff.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Staff.Context.Configuration;

public class GradeConfiguration: IEntityTypeConfiguration<Grade>
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