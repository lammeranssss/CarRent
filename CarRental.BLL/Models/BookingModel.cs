using CarRental.BLL.Abstractions;
using CarRental.DAL.Models.Enums;

namespace CarRental.BLL.Models;
public class BookingModel : BaseModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid CustomerId { get; set; }
    public Guid CarId { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus BookingStatus { get; set; }

    public CustomerModel Customer { get; set; }
    public CarModel Car { get; set; } 
    public RentalModel Rental { get; set; } 
}
