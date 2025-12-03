using CarRental.DAL.Models.Entities;
using CarRental.DAL.Models.Enums;

namespace CarRental.IntegrationTests;

public static class TestDataHelper
{
    public static CarEntity CreateCarEntity(
        Guid? id = null,
        string brand = "TestBrand",
        string model = "TestModel",
        Guid? locationId = null,
        decimal? dailyRate = null)
    {
        return new CarEntity
        {
            Id = id ?? Guid.NewGuid(),
            Brand = brand,
            Model = model,
            Year = 2023,
            LicensePlate = $"TEST{Guid.NewGuid():N}"[..8].ToUpper(),
            Color = "Black",
            LocationId = locationId ?? Guid.NewGuid(),
            CarStatus = CarStatus.Available,
            DailyRate = dailyRate ?? 50m,
            Mileage = 1000m
        };
    }

    public static CustomerEntity CreateCustomerEntity(
        Guid? id = null,
        string firstName = "John",
        string lastName = "Doe")
    {
        var uniqueId = Guid.NewGuid().ToString("N")[..8];
        return new CustomerEntity
        {
            Id = id ?? Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = $"{firstName.ToLower()}.{lastName.ToLower()}{uniqueId}@test.com",
            Phone = "123-456-7890",
            Address = "123 Test Street",
            LicenseNumber = $"LIC{uniqueId}"
        };
    }

    public static LocationEntity CreateLocationEntity(
        Guid? id = null,
        string name = "Test Location")
    {
        return new LocationEntity
        {
            Id = id ?? Guid.NewGuid(),
            Name = name,
            Address = "123 Main St",
            Phone = "555-1234"
        };
    }

    public static BookingEntity CreateBookingEntity(
        Guid? id = null,
        Guid? customerId = null,
        Guid? carId = null,
        decimal? totalPrice = null)
    {
        return new BookingEntity
        {
            Id = id ?? Guid.NewGuid(),
            CustomerId = customerId ?? Guid.NewGuid(),
            CarId = carId ?? Guid.NewGuid(),
            StartDate = DateTime.Today.AddDays(1),
            EndDate = DateTime.Today.AddDays(5),
            TotalPrice = totalPrice ?? 200m,
            BookingStatus = BookingStatus.Pending
        };
    }

    public static RentalEntity CreateRentalEntity(
        Guid? id = null,
        Guid? bookingId = null,
        Guid? pickUpLocationId = null,
        Guid? dropOffLocationId = null)
    {
        return new RentalEntity
        {
            Id = id ?? Guid.NewGuid(),
            BookingId = bookingId ?? Guid.NewGuid(),
            PickUpLocationId = pickUpLocationId ?? Guid.NewGuid(),
            DropOffLocationId = dropOffLocationId ?? Guid.NewGuid(),
            PickUpDate = DateTime.Today.AddDays(1),
            DropOffDate = DateTime.Today.AddDays(5),
            InitialMileage = 1000m,
            FinalMileage = 1200m,
            FinalPrice = 250m
        };
    }
}
