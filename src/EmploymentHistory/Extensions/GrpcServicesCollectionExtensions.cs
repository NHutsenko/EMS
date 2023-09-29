using System.Diagnostics.CodeAnalysis;
using EMS.Configurations;
using EMS.Extensions;
using EMS.Protos;

namespace EMS.EmploymentHistory.Extensions;

[ExcludeFromCodeCoverage]
public static class GrpcServicesCollectionExtensions
{
    public static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        UriConfig config = GetConfig(configuration);

        services.AddGrpcClient<PersonService.PersonServiceClient>(opt => opt.Address = new Uri(config.PersonService!))
            .ConfigureClient();
        
        services.AddGrpcClient<StaffService.StaffServiceClient>(opt => opt.Address = new Uri(config.StaffService!))
            .ConfigureClient();
        
        services.AddGrpcClient<TeamService.TeamServiceClient>(opt => opt.Address = new Uri(config.StructureService!))
            .ConfigureClient();
        
        services.AddGrpcClient<PositionService.PositionServiceClient>(opt => opt.Address = new Uri(config.StructureService!))
            .ConfigureClient();
        
        return services;
    }

    private static UriConfig GetConfig(IConfiguration configuration)
    {
        UriConfig? config = configuration.GetSection(UriConfig.SectionName).Get<UriConfig>();
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrEmpty(config.PersonService);
        ArgumentException.ThrowIfNullOrEmpty(config.StaffService);
        ArgumentException.ThrowIfNullOrEmpty(config.StructureService);
        return config;
    }
}