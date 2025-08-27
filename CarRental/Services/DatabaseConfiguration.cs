using CarRental.ntier.DAL;
using CarRental.ntier.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using CarRental.ntier.DAL.DataContext;

namespace CarRental.ntier.API.Services.Configurations;
public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabaseServices(
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

        return services;
    }
}
