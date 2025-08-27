namespace CarRental.ntier.API.Models.Requests.Bookings
{
    public record CreateBookingRequest(
        DateTime StartDate,
        DateTime EndDate,
        Guid CustomerId,
        Guid CarId
    );
}
