using CarRental.DAL.DataContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;

namespace CarRental.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly InMemoryDatabaseRoot _root = new();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptorsToRemove = services.Where(d =>
    d.ServiceType == typeof(DbContextOptions<CarRentalDbContext>) ||
    d.ServiceType == typeof(CarRentalDbContext) ||
    d.ServiceType.Name.Contains("DatabaseProvider") ||
    d.ServiceType.Name.Contains("DbContextOptions") ||
    d.ServiceType.Name.Contains("IDbContextOptionsConfiguration") ||
    d.ImplementationType?.Namespace?.Contains("Npgsql") == true
).ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }
            services.AddDbContext<CarRentalDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase", _root);
            });

            var authDescriptors = services.Where(d =>
                d.ServiceType.FullName?.Contains("Authentication") == true).ToList();
            foreach (var descriptor in authDescriptors)
            {
                services.Remove(descriptor);
            }

            services.AddAuthentication("TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
        });
    }
}
