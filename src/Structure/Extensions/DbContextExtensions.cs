using System.Diagnostics.CodeAnalysis;
using EMS.Staff.Context;
using Microsoft.EntityFrameworkCore;

namespace EMS.Staff.Extensions;

[ExcludeFromCodeCoverage]
public static class DbContextExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string? dbConnectionString = configuration.GetConnectionString("Structure");
        ArgumentException.ThrowIfNullOrEmpty(dbConnectionString);

        services.AddDbContext<StaffContext>(opt => opt.UseNpgsql(dbConnectionString));
        
        return services;
    }
}