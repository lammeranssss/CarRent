using CarRental.DAL.Abstractions;
using CarRental.DAL.DataContext;
using CarRental.DAL.Models.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarRental.DAL.DI;

public static class DependencyRegister
{
    public static IServiceCollection AddDalDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<CarRentalDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MapEnum<BookingStatusEnum>();
                npgsqlOptions.MapEnum<CarStatusEnum>();
            }));

        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();
        return services;
    }
}
