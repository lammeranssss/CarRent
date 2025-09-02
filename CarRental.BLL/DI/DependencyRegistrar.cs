using CarRental.BLL.Abstractions;
using CarRental.BLL.Services;
using CarRental.DAL.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.BLL.DI;

public static class DependencyRegistrar
{
    public static IServiceCollection AddBllDependencies(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(Mapping.BllMappingProfile));
        services.AddDalDependencies(configuration);
        services.AddScoped(typeof(IGenericService<>), typeof(GenericService<,>));
        return services;
    }
}
