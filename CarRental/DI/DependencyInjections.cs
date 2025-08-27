using CarRental.ntier.DAL.Abstractions;
using CarRental.ntier.DAL.DataContext;
using CarRental.ntier.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarRental.ntier.API.DI;
public static class DependencyInjections
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
