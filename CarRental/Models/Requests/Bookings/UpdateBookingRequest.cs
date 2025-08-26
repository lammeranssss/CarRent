namespace CarRental.ntier.API.Models.Requests.Bookings
{
    public class UpdateBookingRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CarId { get; set; }
    }
}
