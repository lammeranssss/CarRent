using System.Net;
using System.Text;
using System.Text.Json;
using CarRental.API.Models.Requests.Rentals;
using CarRental.API.Models.Responses.Rentals;
using CarRental.DAL.DataContext;
using CarRental.DAL.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace CarRental.IntegrationTests.Controllers;

public class RentalsControllerTests(CustomWebApplicationFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetAll_WhenRentalsExist_ReturnsRentalsList()
    {
        // Arrange
        var (booking, pickUpLocation, dropOffLocation) = await CreatePrerequisitesAsync();
        var rentals = new[]
        {
            TestDataHelper.CreateRentalEntity(bookingId: booking.Id, pickUpLocationId: pickUpLocation.Id, dropOffLocationId: dropOffLocation.Id),
            TestDataHelper.CreateRentalEntity(bookingId: booking.Id, pickUpLocationId: pickUpLocation.Id, dropOffLocationId: dropOffLocation.Id)
        };
        await AddEntitiesAsync(rentals);

        // Act
        var response = await Client.GetAsync("/api/rentals");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = Deserialize<List<RentalResponse>>(await response.Content.ReadAsStringAsync());

        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetById_WhenRentalExists_ReturnsRental()
    {
        // Arrange
        var (booking, pickUpLocation, dropOffLocation) = await CreatePrerequisitesAsync();
        var rental = await AddEntityAsync(TestDataHelper.CreateRentalEntity(bookingId: booking.Id, pickUpLocationId: pickUpLocation.Id, dropOffLocationId: dropOffLocation.Id));

        // Act
        var response = await Client.GetAsync($"/api/rentals/{rental.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = Deserialize<RentalResponse>(await response.Content.ReadAsStringAsync());

        result.ShouldNotBeNull();
        result.Id.ShouldBe(rental.Id);
    }

    [Fact]
    public async Task GetById_WhenRentalNotExists_ReturnsNotFound()
    {
        // Act
        var response = await Client.GetAsync($"/api/rentals/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsRental()
    {
        // Arrange
        var (booking, pickUpLocation, dropOffLocation) = await CreatePrerequisitesAsync();
        var rentalData = TestDataHelper.CreateRentalEntity(bookingId: booking.Id, pickUpLocationId: pickUpLocation.Id, dropOffLocationId: dropOffLocation.Id);

        var request = new CreateRentalRequest(
            BookingId: rentalData.BookingId,
            PickUpDate: rentalData.PickUpDate,
            DropOffDate: rentalData.DropOffDate,
            PickUpLocationId: rentalData.PickUpLocationId,
            DropOffLocationId: rentalData.DropOffLocationId,
            InitialMileage: rentalData.InitialMileage
        );
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("/api/rentals", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = Deserialize<RentalResponse>(await response.Content.ReadAsStringAsync());

        result.ShouldNotBeNull();
        result.BookingId.ShouldBe(request.BookingId);
    }

    [Fact]
    public async Task Update_WithValidRequest_ReturnsUpdatedRental()
    {
        // Arrange
        var (booking, pickUpLocation, dropOffLocation) = await CreatePrerequisitesAsync();
        var rental = await AddEntityAsync(TestDataHelper.CreateRentalEntity(bookingId: booking.Id, pickUpLocationId: pickUpLocation.Id, dropOffLocationId: dropOffLocation.Id));

        var updatedRentalData = TestDataHelper.CreateRentalEntity();

        var request = new UpdateRentalRequest(
            PickUpDate: updatedRentalData.PickUpDate.AddDays(1),
            DropOffDate: updatedRentalData.DropOffDate.AddDays(1),
            PickUpLocationId: pickUpLocation.Id,
            DropOffLocationId: dropOffLocation.Id,
            FinalMileage: rental.InitialMileage + 500
        );
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PutAsync($"/api/rentals/{rental.Id}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = Deserialize<RentalResponse>(await response.Content.ReadAsStringAsync());

        result.ShouldNotBeNull();
        result.Id.ShouldBe(rental.Id);
        result.FinalMileage.ShouldBe(request.FinalMileage);
    }

    [Fact]
    public async Task Delete_WhenRentalExists_RemovesRental()
    {
        // Arrange
        var (booking, pickUpLocation, dropOffLocation) = await CreatePrerequisitesAsync();
        var rental = await AddEntityAsync(TestDataHelper.CreateRentalEntity(bookingId: booking.Id, pickUpLocationId: pickUpLocation.Id, dropOffLocationId: dropOffLocation.Id));

        // Act
        var response = await Client.DeleteAsync($"/api/rentals/{rental.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var scope = CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
        var deletedRental = await dbContext.Rentals.FindAsync(rental.Id);
        deletedRental.ShouldBeNull();
    }

    private async Task<(BookingEntity booking, LocationEntity pickUpLocation, LocationEntity dropOffLocation)> CreatePrerequisitesAsync()
    {
        var location = await AddEntityAsync(TestDataHelper.CreateLocationEntity());
        var customer = await AddEntityAsync(TestDataHelper.CreateCustomerEntity());
        var car = await AddEntityAsync(TestDataHelper.CreateCarEntity(locationId: location.Id));
        var booking = await AddEntityAsync(TestDataHelper.CreateBookingEntity(customerId: customer.Id, carId: car.Id));
        var pickUpLocation = await AddEntityAsync(TestDataHelper.CreateLocationEntity(name: "PickUp Location"));
        var dropOffLocation = await AddEntityAsync(TestDataHelper.CreateLocationEntity(name: "DropOff Location"));

        return (booking, pickUpLocation, dropOffLocation);
    }
}
