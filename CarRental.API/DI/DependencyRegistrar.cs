using CarRental.BLL.DI;
using FluentValidation;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace CarRental.API.DI;

public static class DependencyRegistrar
{
    public static IServiceCollection AddApiDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(Mapping.ApiMappingProfile));
        services.AddBllDependencies(configuration);
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(DependencyRegistrar)));
        return services;
    }
}
