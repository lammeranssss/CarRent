using CarRental.BLL.DI;
using FluentValidation;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CarRental.API.DI;

public static class DependencyRegistrar
{
    public static IServiceCollection AddApiDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(cfg => { cfg.AddMaps(typeof(Mapping.ApiMappingProfile).Assembly); });
        services.AddBllDependencies(configuration);
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(DependencyRegistrar)));
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = configuration["Auth0:Domain"];
        options.Audience = configuration["Auth0:Audience"];
    });
        return services;
    }
}
