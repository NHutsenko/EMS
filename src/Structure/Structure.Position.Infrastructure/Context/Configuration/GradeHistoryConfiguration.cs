using EMS.Structure.Position.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Structure.Position.Infrastructure.Context.Configuration;

internal sealed class GradeHistoryConfiguration: IEntityTypeConfiguration<GradeHistory>
{
    public void Configure(EntityTypeBuilder<GradeHistory> builder)
    {
        builder.ToTable("GradeHistory", "dbo")
            .HasKey(e => e.Id);
    }
}