using System.Diagnostics.CodeAnalysis;
using EMS.Person.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Person.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class DbContextExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string? dbConnectionString = configuration.GetConnectionString("Person");
        ArgumentException.ThrowIfNullOrEmpty(dbConnectionString);

        services.AddDbContext<PersonContext>(opt => opt.UseNpgsql(dbConnectionString));
        
        return services;
    }
}