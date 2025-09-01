using Microsoft.Extensions.DependencyInjection;

namespace CarRental.BLL.Extensions;
public static class BllMappingExtensions
{
    public static IServiceCollection AddBllMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Mapping.DalMappingProfile));
        return services;
    }
}
