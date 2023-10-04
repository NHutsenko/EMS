using System.Diagnostics.CodeAnalysis;
using EMS.Structure.Context;
using Microsoft.EntityFrameworkCore;

namespace EMS.Structure.Extensions;

[ExcludeFromCodeCoverage]
public static class DbContextExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string? dbConnectionString = configuration.GetConnectionString("Structure");
        ArgumentException.ThrowIfNullOrEmpty(dbConnectionString);

        services.AddDbContext<StructureContext>(opt => opt.UseNpgsql(dbConnectionString));
        
        return services;
    }
}