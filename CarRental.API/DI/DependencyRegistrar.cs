using CarRental.BLL.DI;
using FluentValidation.AspNetCore;
using FluentValidation;

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
        services.AddValidatorsFromAssemblyContaining<Program>();
        return services;    
    }
}
