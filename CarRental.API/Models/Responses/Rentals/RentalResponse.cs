namespace CarRental.API.Models.Responses.Rentals;
public record RentalResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    Guid BookingId,
    DateTime PickUpDate,
    DateTime DropOffDate,
    Guid PickUpLocationId,
    Guid DropOffLocationId,
    decimal InitialMileage,
    decimal FinalMileage,
    decimal FinalPrice,
    string PickUpLocationName,
    string DropOffLocationName,
    decimal MileageUsed,
    bool IsActive,
    bool HasExceededMileageLimit
);
