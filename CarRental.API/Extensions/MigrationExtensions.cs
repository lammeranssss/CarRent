using CarRental.DAL.DataContext;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();

            var strategy = context.Database.CreateExecutionStrategy();

            strategy.Execute(context.Database.Migrate);
        }
    }
}
