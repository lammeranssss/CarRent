using CarRental.BLL.Abstractions;

namespace CarRental.BLL.Models;
public class RentalModel : BaseModel
{
    public Guid BookingId { get; set; }
    public DateTime PickUpDate { get; set; }
    public DateTime DropOffDate { get; set; }
    public Guid PickUpLocationId { get; set; }
    public Guid DropOffLocationId { get; set; }
    public decimal InitialMileage { get; set; }
    public decimal FinalMileage { get; set; }
    public decimal FinalPrice { get; set; }

    public BookingModel Booking { get; set; } = new();
    public LocationModel PickUpLocation { get; set; } = new();
    public LocationModel DropOffLocation { get; set; } = new();
}
