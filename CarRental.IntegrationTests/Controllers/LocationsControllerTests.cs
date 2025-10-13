using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CarRental.API.Models.Requests.Locations;
using CarRental.API.Models.Responses.Locations;
using CarRental.DAL.DataContext;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace CarRental.IntegrationTests.Controllers;

public class LocationsControllerTests : BaseIntegrationTest
{
    public LocationsControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
    }
    [Fact]
    public async Task GetAll_WhenLocationsExist_ReturnsLocationsList()
    {
        // Arrange
        var locations = new[]
        {
            TestDataHelper.CreateLocationEntity(),
            TestDataHelper.CreateLocationEntity()
        };
        await AddEntitiesAsync(locations);

        // Act
        var response = await Client.GetAsync("/api/locations");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = Deserialize<List<LocationResponse>>(await response.Content.ReadAsStringAsync());

        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetById_WhenLocationExists_ReturnsLocation()
    {
        // Arrange
        var location = await AddEntityAsync(TestDataHelper.CreateLocationEntity());

        // Act
        var response = await Client.GetAsync($"/api/locations/{location.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = Deserialize<LocationResponse>(await response.Content.ReadAsStringAsync());

        result.ShouldNotBeNull();
        result.Id.ShouldBe(location.Id);
    }

    [Fact]
    public async Task GetById_WhenLocationNotExists_ReturnsNotFound()
    {
        // Act
        var response = await Client.GetAsync($"/api/locations/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Create_WithValidRequest_ReturnsCreatedLocation()
    {
        // Arrange
        var locationData = TestDataHelper.CreateLocationEntity();
        var request = new CreateLocationRequest(
            Name: locationData.Name,
            Address: locationData.Address,
            Phone: locationData.Phone
        );
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("/api/locations", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = Deserialize<LocationResponse>(await response.Content.ReadAsStringAsync());

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
    }

    [Fact]
    public async Task Update_WithValidRequest_ReturnsUpdatedLocation()
    {
        // Arrange
        var location = await AddEntityAsync(TestDataHelper.CreateLocationEntity());
        var updatedLocationData = TestDataHelper.CreateLocationEntity(name: "Updated Location");

        var request = new CreateLocationRequest(
            Name: updatedLocationData.Name,
            Address: updatedLocationData.Address,
            Phone: updatedLocationData.Phone
        );
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PutAsync($"/api/locations/{location.Id}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = Deserialize<LocationResponse>(await response.Content.ReadAsStringAsync());

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
    }

    [Fact]
    public async Task Delete_WhenLocationExists_RemovesLocation()
    {
        // Arrange
        var location = await AddEntityAsync(TestDataHelper.CreateLocationEntity());

        // Act
        var response = await Client.DeleteAsync($"/api/locations/{location.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var scope = CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
        var deletedLocation = await dbContext.Locations.FindAsync(location.Id);
        deletedLocation.ShouldBeNull();
    }
}
