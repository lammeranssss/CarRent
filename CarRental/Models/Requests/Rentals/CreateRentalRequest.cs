namespace CarRental.API.Models.Requests.Rentals;
public record CreateRentalRequest(
    Guid BookingId,
    DateTime PickUpDate,
    DateTime DropOffDate,
    Guid PickUpLocationId,
    Guid DropOffLocationId,
    decimal InitialMileage
);
