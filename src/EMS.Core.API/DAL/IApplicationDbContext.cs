using System.Threading;
using System.Threading.Tasks;
using EMS.Core.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Core.API.DAL
{
    public interface IApplicationDbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<DayOff> DaysOff { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<OtherPayment> OtherPayments { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PersonPhoto> Photos { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Team> Teams { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default);
    }
}
