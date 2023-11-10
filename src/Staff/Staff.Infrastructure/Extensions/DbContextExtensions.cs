using System.Diagnostics.CodeAnalysis;
using EMS.Staff.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Staff.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class DbContextExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string? dbConnectionString = configuration.GetConnectionString("Staff");
        ArgumentException.ThrowIfNullOrEmpty(dbConnectionString);

        services.AddDbContext<StaffContext>(opt => opt.UseNpgsql(dbConnectionString));
        
        return services;
    }
}