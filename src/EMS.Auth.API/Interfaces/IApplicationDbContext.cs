using System.Threading;
using System.Threading.Tasks;
using EMS.Auth.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Auth.API.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<UserToken> Tokens { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default);
    }
}
