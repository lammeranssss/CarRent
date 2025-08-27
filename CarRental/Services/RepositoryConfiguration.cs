using CarRental.ntier.DAL.Abstractions;
using CarRental.ntier.DAL.DataContext;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.ntier.API.Services;

public static class RepositoryConfiguration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        return services;
    }
}
