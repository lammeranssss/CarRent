using CarRental.DAL.Abstractions;
using CarRental.DAL.DataContext;
using CarRental.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.DAL.DI;
public static class DependencyRegistrar
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

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        return services;
    }
}
