using CarRental.ntier.DAL.Abstractions;
using CarRental.ntier.DAL.Models.Enums;

namespace CarRental.ntier.DAL.Models.Entities;
public class CarEntity : BaseEntity
{
    public string Brand { get; set; }

    public string Model { get; set; }

    public int Year { get; set; }

    public string LicensePlate { get; set; }

    public string Color { get; set; }

    public Guid LocationId { get; set; }

    public CarStatusEnum CarStatus { get; set; }

    public decimal DailyRate { get; set; }

    public decimal Mileage { get; set; }

    public LocationEntity? Location { get; set; }
    public ICollection<BookingEntity> Bookings { get; set; } = [];
}
