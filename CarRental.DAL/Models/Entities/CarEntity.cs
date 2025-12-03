using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Enums;

namespace CarRental.DAL.Models.Entities;
public class CarEntity : BaseEntity
{
    public string Brand { get; set; }

    public string Model { get; set; }

    public int Year { get; set; }

    public string LicensePlate { get; set; }

    public string Color { get; set; }

    public Guid LocationId { get; set; }

    public CarStatus CarStatus { get; set; }

    public decimal DailyRate { get; set; }

    public decimal Mileage { get; set; }

    public LocationEntity? Location { get; set; }
    public ICollection<BookingEntity> Bookings { get; set; } = [];
}
