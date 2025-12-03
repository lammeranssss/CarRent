using CarRental.BLL.Abstractions;
using CarRental.DAL.Models.Enums;

namespace CarRental.BLL.Models;
public class CarModel : BaseModel
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

    public LocationModel Location { get; set; } = new();
    public ICollection<BookingModel> Bookings { get; set; } = [];
}
