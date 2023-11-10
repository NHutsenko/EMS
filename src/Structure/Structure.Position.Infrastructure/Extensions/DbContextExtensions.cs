using System.Diagnostics.CodeAnalysis;
using EMS.Structure.Position.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Structure.Position.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class DbContextExtensions
{
    public static IServiceCollection AddPositionDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string? dbConnectionString = configuration.GetConnectionString("Structure");
        ArgumentException.ThrowIfNullOrEmpty(dbConnectionString);

        services.AddDbContext<PositionContext>(opt => opt.UseNpgsql(dbConnectionString));
        
        return services;
    }
}