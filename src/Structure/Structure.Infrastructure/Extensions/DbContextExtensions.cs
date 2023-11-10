using System.Diagnostics.CodeAnalysis;
using EMS.Structure.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Structure.Infrastructure.Extensions;

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