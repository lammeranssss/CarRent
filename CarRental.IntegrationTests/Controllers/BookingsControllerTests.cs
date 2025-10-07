using System.Net;
using System.Text;
using System.Text.Json;
using CarRental.API.Models.Requests.Bookings;
using CarRental.API.Models.Responses.Bookings;
using CarRental.DAL.DataContext;
using CarRental.DAL.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace CarRental.IntegrationTests.Controllers;

public class BookingsControllerTests(CustomWebApplicationFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetAll_WhenBookingsExist_ReturnsBookingsList()
    {
        // Arrange
        var (customer, car) = await CreatePrerequisitesAsync();
        var bookings = new[]
        {
            TestDataHelper.CreateBookingEntity(customerId: customer.Id, carId: car.Id),
            TestDataHelper.CreateBookingEntity(customerId: customer.Id, carId: car.Id)
        };
        await AddEntitiesAsync(bookings);

        // Act
        var response = await Client.GetAsync("/api/bookings");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var result = Deserialize<List<BookingResponse>>(content);

        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetById_WhenBookingExists_ReturnsBooking()
    {
        // Arrange
        var (customer, car) = await CreatePrerequisitesAsync();
        var booking = await AddEntityAsync(TestDataHelper.CreateBookingEntity(customerId: customer.Id, carId: car.Id));

        // Act
        var response = await Client.GetAsync($"/api/bookings/{booking.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var result = Deserialize<BookingResponse>(content);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(booking.Id);
    }

    [Fact]
    public async Task GetById_WhenBookingNotExists_ReturnsNotFound()
    {
        // Act
        var response = await Client.GetAsync($"/api/bookings/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsBooking()
    {
        // Arrange
        var (customer, car) = await CreatePrerequisitesAsync();
        var bookingData = TestDataHelper.CreateBookingEntity(customerId: customer.Id, carId: car.Id);

        var request = new CreateBookingRequest(
            StartDate: bookingData.StartDate,
            EndDate: bookingData.EndDate,
            CustomerId: bookingData.CustomerId,
            CarId: bookingData.CarId
        );
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("/api/bookings", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = Deserialize<BookingResponse>(responseContent);

        result.ShouldNotBeNull();
        result.CustomerId.ShouldBe(customer.Id);
        result.CarId.ShouldBe(car.Id);

        using var scope = CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
        var dbBooking = await dbContext.Bookings.FindAsync(result.Id);
        dbBooking.ShouldNotBeNull();
    }

    [Fact]
    public async Task Update_WhenBookingExists_ReturnsUpdatedBooking()
    {
        // Arrange
        var (customer, car) = await CreatePrerequisitesAsync();
        var booking = await AddEntityAsync(TestDataHelper.CreateBookingEntity(customerId: customer.Id, carId: car.Id));

        var updatedEndDate = DateTime.Today.AddDays(10);
        var request = new CreateBookingRequest(
            StartDate: booking.StartDate,
            EndDate: updatedEndDate,
            CustomerId: booking.CustomerId,
            CarId: booking.CarId
        );
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PutAsync($"/api/bookings/{booking.Id}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = Deserialize<BookingResponse>(responseContent);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(booking.Id);
        result.EndDate.Date.ShouldBe(updatedEndDate.Date);
    }

    [Fact]
    public async Task Delete_WhenBookingExists_RemovesBooking()
    {
        // Arrange
        var (customer, car) = await CreatePrerequisitesAsync();
        var booking = await AddEntityAsync(TestDataHelper.CreateBookingEntity(customerId: customer.Id, carId: car.Id));

        // Act
        var response = await Client.DeleteAsync($"/api/bookings/{booking.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var scope = CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
        var deletedBooking = await dbContext.Bookings.FindAsync(booking.Id);
        deletedBooking.ShouldBeNull();
    }

    private async Task<(CustomerEntity customer, CarEntity car)> CreatePrerequisitesAsync()
    {
        var location = await AddEntityAsync(TestDataHelper.CreateLocationEntity());
        var customer = await AddEntityAsync(TestDataHelper.CreateCustomerEntity());
        var car = await AddEntityAsync(TestDataHelper.CreateCarEntity(locationId: location.Id));
        return (customer, car);
    }
}
