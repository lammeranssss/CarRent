namespace CarRental.API.Extensions;
public static class ApiMappingExtensions
{
    public static IServiceCollection AddApiMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Mapping.ApiMappingProfile));
        return services;
    }
}
