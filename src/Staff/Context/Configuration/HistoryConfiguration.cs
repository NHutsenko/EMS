using EMS.Staff.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Staff.Context.Configuration;

public sealed class HistoryConfiguration: IEntityTypeConfiguration<History>
{
    public void Configure(EntityTypeBuilder<History> builder)
    {
        builder.ToTable("StaffHistory", "dbo")
            .HasKey(e => e.Id);

        builder.Property(e => e.CreatedOn)
            .HasColumnType("date");
    }
}