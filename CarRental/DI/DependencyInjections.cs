using CarRental.ntier.DAL.Abstractions;
using CarRental.ntier.DAL.DataContext;

namespace CarRental.ntier.API.DI;
public static class DependencyInjections
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        return services;
    }
}
