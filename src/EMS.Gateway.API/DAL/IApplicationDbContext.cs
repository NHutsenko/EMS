using System.Threading;
using System.Threading.Tasks;
using EMS.Gateway.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Gateway.API.DAL
{
    public interface IApplicationDbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet <DayOff> DaysOff { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default);
    }
}
