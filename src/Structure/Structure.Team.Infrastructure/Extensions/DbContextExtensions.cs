using System.Diagnostics.CodeAnalysis;
using EMS.Structure.Team.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Structure.Team.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class DbContextExtensions
{
    public static IServiceCollection AddTeamDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string? dbConnectionString = configuration.GetConnectionString("Structure");
        ArgumentException.ThrowIfNullOrEmpty(dbConnectionString);

        services.AddDbContext<TeamContext>(opt => opt.UseNpgsql(dbConnectionString));
        
        return services;
    }
}