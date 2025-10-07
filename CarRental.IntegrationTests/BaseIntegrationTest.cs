using System.Text.Json;
using CarRental.DAL.DataContext;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.IntegrationTests;

public abstract class BaseIntegrationTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory = factory;
    protected readonly HttpClient Client = factory.CreateClient();

    public Task InitializeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
        
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        
        return Task.CompletedTask;
    }
    public Task DisposeAsync() => Task.CompletedTask;

    protected async Task<TEntity> AddEntityAsync<TEntity>(TEntity entity) where TEntity : class
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
        dbContext.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    protected async Task<List<TEntity>> AddEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
        dbContext.AddRange(entities);
        await dbContext.SaveChangesAsync();
        return entities.ToList();
    }
    
    protected IServiceScope CreateScope() => _factory.Services.CreateScope();

    protected static T? Deserialize<T>(string content) =>
        JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
}