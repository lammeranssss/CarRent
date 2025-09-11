using CarRental.BLL.DI;
using FluentValidation;
using System.Reflection;

namespace CarRental.API.DI;

public static class DependencyRegistrar
{
    public static IServiceCollection AddApiDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(Mapping.ApiMappingProfile));
        services.AddBllDependencies(configuration);
        services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(DependencyRegistrar)));
        return services;
    }
}
