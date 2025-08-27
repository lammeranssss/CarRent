namespace CarRental.ntier.API.Models.Requests.Rentals;
public record UpdateRentalRequest(
    DateTime PickUpDate,
    DateTime DropOffDate,
    Guid PickUpLocationId,
    Guid DropOffLocationId,
    decimal FinalMileage
);
