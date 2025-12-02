using CarRental.BLL.DI;
using FluentValidation;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CarRental.Utilities; 
using CarRental.Utilities.Abstractions; 
using CarRental.Utilities.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace CarRental.API.DI;

public static class DependencyRegister
{
    public static IServiceCollection AddApiDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(cfg => { cfg.AddMaps(typeof(Mapping.ApiMappingProfile).Assembly); });
        services.AddBllDependencies(configuration);
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(DependencyRegister)));
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["Auth0:Domain"];
                options.Audience = configuration["Auth0:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "https://api.carrental.com/roles",
                };
            });
        services.AddMassTransitForSending(configuration);
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddHttpContextAccessor();
        services.AddScoped<ITraceIdProvider, TraceIdProvider>();

        return services;
    }
}
