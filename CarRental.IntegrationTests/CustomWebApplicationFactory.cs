using CarRental.DAL.DataContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace CarRental.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly InMemoryDatabaseRoot _root = new();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTest");

        builder.ConfigureServices(services =>
        {
            var descriptorsToRemove = services
                .Where(d =>
                    d.ServiceType == typeof(CarRentalDbContext) ||

                    d.ServiceType == typeof(DbContextOptions<CarRentalDbContext>) ||

                    d.ImplementationType?.Namespace?.StartsWith("Npgsql.EntityFrameworkCore.PostgreSQL") == true)
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }
            var healthCheckService = services.FirstOrDefault(d => d.ServiceType.Name.Contains("HealthCheckService"));
            if (healthCheckService != null)
            {
                services.RemoveAll(typeof(IHostedService)); 
                services.AddHealthChecks(); 
            }

            services.AddDbContext<CarRentalDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase", _root);
            });
        });
    }
}