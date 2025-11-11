namespace CarRental.API.Models.Requests.Rentals;
public record UpdateRentalRequest(
    Guid BookingId,
    DateTime PickUpDate,
    DateTime DropOffDate,
    Guid PickUpLocationId,
    Guid DropOffLocationId,
    decimal InitialMileage,
    decimal FinalMileage,
    decimal FinalPrice
);
