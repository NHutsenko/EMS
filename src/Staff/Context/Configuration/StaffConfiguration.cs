using EMS.Staff.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Staff.Context.Configuration;

public sealed class StaffConfiguration: IEntityTypeConfiguration<Models.Staff>
{
    public void Configure(EntityTypeBuilder<Models.Staff> builder)
    {
        builder.ToTable("Staff", "dbo")
            .HasKey(e => e.Id);

        builder.HasOne(e => e.History)
            .WithOne(e => e.Staff)
            .HasForeignKey<History>(e => e.StaffId);
    }
}