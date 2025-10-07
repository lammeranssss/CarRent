using CarRental.BLL.Abstractions;
using CarRental.BLL.Services;
using CarRental.DAL.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace CarRental.BLL.DI;

public static class DependencyRegistrar
{
    public static IServiceCollection AddBllDependencies(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddAutoMapper(cfg => { cfg.AddMaps(typeof(Mapping.BllMappingProfile).Assembly); });
        services.AddDalDependencies(configuration);
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IRentalService, RentalService>();
        return services;
    }
}
