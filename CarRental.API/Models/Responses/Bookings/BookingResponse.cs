namespace CarRental.API.Models.Responses.Bookings;
public record BookingResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime StartDate,
    DateTime EndDate,
    Guid CustomerId,
    Guid CarId,
    decimal TotalPrice,
    string BookingStatus,
    string CustomerName,
    string CarDetails,
    int DurationInDays,
    bool IsActive,
    bool CanBeCancelled
);
