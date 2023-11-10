using EMS.Staff.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Staff.Infrastructure.Context.Configuration;

internal sealed class StaffConfiguration: IEntityTypeConfiguration<Domain.Staff>
{
    public void Configure(EntityTypeBuilder<Domain.Staff> builder)
    {
        builder.ToTable("Staff", "dbo")
            .HasKey(e => e.Id);

        builder.HasOne(e => e.History)
            .WithOne(e => e.Staff)
            .HasForeignKey<History>(e => e.StaffId);
    }
}