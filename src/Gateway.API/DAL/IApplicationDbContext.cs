using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EMS.Gateway.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EMS.Gateway.API.DAL
{
    public interface IApplicationDbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Position> Positions { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default);
    }
}
