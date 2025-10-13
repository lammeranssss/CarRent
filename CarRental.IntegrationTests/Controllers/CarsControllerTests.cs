using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CarRental.API.Models.Requests.Cars;
using CarRental.API.Models.Responses.Cars;
using CarRental.DAL.DataContext;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace CarRental.IntegrationTests.Controllers;

public class CarsControllerTests : BaseIntegrationTest
{
    private readonly CustomWebApplicationFactory _factory;
    public CarsControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
        _factory = factory;
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
    }
    [Fact]
    public async Task GetAll_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var unauthenticatedClient = _factory.CreateClient();

        // Act
        var response = await unauthenticatedClient.GetAsync("/api/cars");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAll_WhenCarsExist_ReturnsCarsList()
    {
        // Arrange
        var location = await AddEntityAsync(TestDataHelper.CreateLocationEntity());
        var cars = new[]
        {
            TestDataHelper.CreateCarEntity(locationId: location.Id),
            TestDataHelper.CreateCarEntity(locationId: location.Id)
        };
        await AddEntitiesAsync(cars);

        // Act
        var response = await Client.GetAsync("/api/cars");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var result = Deserialize<List<CarResponse>>(content);

        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetById_WhenCarExists_ReturnsCar()
    {
        // Arrange
        var location = await AddEntityAsync(TestDataHelper.CreateLocationEntity());
        var car = await AddEntityAsync(TestDataHelper.CreateCarEntity(locationId: location.Id));

        // Act
        var response = await Client.GetAsync($"/api/cars/{car.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var result = Deserialize<CarResponse>(content);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(car.Id);
    }

    [Fact]
    public async Task GetById_WhenCarNotExists_ReturnsNotFound()
    {
        // Act
        var response = await Client.GetAsync($"/api/cars/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Create_WithValidRequest_ReturnsCreatedCar()
    {
        // Arrange
        var location = await AddEntityAsync(TestDataHelper.CreateLocationEntity());
        var carData = TestDataHelper.CreateCarEntity(locationId: location.Id);

        var request = new CreateCarRequest(
            Brand: carData.Brand,
            Model: carData.Model,
            Year: carData.Year,
            LicensePlate: carData.LicensePlate,
            Color: carData.Color,
            LocationId: carData.LocationId,
            DailyRate: carData.DailyRate
        );
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("/api/cars", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = Deserialize<CarResponse>(await response.Content.ReadAsStringAsync());

        result.ShouldNotBeNull();
        result.Brand.ShouldBe(request.Brand);
        result.Model.ShouldBe(request.Model);
    }

    [Fact]
    public async Task Update_WithValidRequest_ReturnsUpdatedCar()
    {
        // Arrange
        var location = await AddEntityAsync(TestDataHelper.CreateLocationEntity());
        var car = await AddEntityAsync(TestDataHelper.CreateCarEntity(locationId: location.Id));

        var updatedCarData = TestDataHelper.CreateCarEntity(brand: "UpdatedBrand", model: "UpdatedModel");

        var request = new CreateCarRequest(
            Brand: updatedCarData.Brand,
            Model: updatedCarData.Model,
            Year: car.Year,
            LicensePlate: car.LicensePlate,
            Color: car.Color,
            LocationId: car.LocationId,
            DailyRate: car.DailyRate
        );
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PutAsync($"/api/cars/{car.Id}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = Deserialize<CarResponse>(await response.Content.ReadAsStringAsync());

        result.ShouldNotBeNull();
        result.Brand.ShouldBe(request.Brand);
        result.Model.ShouldBe(request.Model);
    }

    [Fact]
    public async Task Delete_WhenCarExists_RemovesCar()
    {
        // Arrange
        var location = await AddEntityAsync(TestDataHelper.CreateLocationEntity());
        var car = await AddEntityAsync(TestDataHelper.CreateCarEntity(locationId: location.Id));

        // Act
        var response = await Client.DeleteAsync($"/api/cars/{car.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var scope = CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
        var deletedCar = await dbContext.Cars.FindAsync(car.Id);
        deletedCar.ShouldBeNull();
    }
}
