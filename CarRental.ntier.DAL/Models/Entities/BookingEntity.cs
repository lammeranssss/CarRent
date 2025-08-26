using CarRental.ntier.DAL.Abstractions;
using CarRental.ntier.DAL.Models.Enums;

namespace CarRental.ntier.DAL.Models.Entities
{
    public class BookingEntity : BaseEntity
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Guid CustomerId { get; set; }

        public Guid CarId { get; set; }

        public decimal TotalPrice { get; set; }

        public BookingStatusEnum BookingStatus { get; set; } 

        public CustomerEntity? Customer { get; set; }

        public CarEntity? Car { get; set; }

        public RentalEntity? Rental { get; set; }
    }
}
