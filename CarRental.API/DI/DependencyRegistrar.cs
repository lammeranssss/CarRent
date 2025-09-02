using CarRental.BLL.DI;

namespace CarRental.API.DI;

public static class DependencyRegistrar
{
    public static IServiceCollection AddApiDependencies(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(Mapping.ApiMappingProfile));
        services.AddBllDependencies(configuration);
        return services;
    }
}
