using System.Diagnostics.CodeAnalysis;
using EMS.Person.Context;
using Microsoft.EntityFrameworkCore;

namespace EMS.Person.Extensions;

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