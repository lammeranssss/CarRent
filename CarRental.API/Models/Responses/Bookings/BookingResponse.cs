namespace CarRental.API.Models.Responses.Bookings;

public class BookingResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid CustomerId { get; set; }
    public Guid CarId { get; set; }
    public decimal TotalPrice { get; set; }
    public string BookingStatus { get; set; }
    public string CustomerName { get; set; }
    public string CarDetails { get; set; }
    public int DurationInDays { get; set; }
    public bool IsActive { get; set; }
    public bool CanBeCancelled { get; set; }
}
