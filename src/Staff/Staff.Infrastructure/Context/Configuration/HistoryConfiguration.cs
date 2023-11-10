using EMS.Staff.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Staff.Infrastructure.Context.Configuration;

internal sealed class HistoryConfiguration: IEntityTypeConfiguration<History>
{
    public void Configure(EntityTypeBuilder<History> builder)
    {
        
        builder.ToTable("StaffHistory", "dbo")
            .HasKey(e => e.Id);

        builder.Property(e => e.CreatedOn)
            .HasColumnType("date");
    }
}