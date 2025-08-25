using CarRental.ntier.DAL.Abstractions;
using CarRental.ntier.DAL.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.ntier.DAL.Models.Entities
{
    [Table("Bookings")]
    public class BookingEntity : BaseEntity
    {

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Guid CustomerId { get; set; }

        public Guid CarId { get; set; }

        public decimal TotalPrice { get; set; }

        public BookingStatusEnum BookingStatus { get; set; } = BookingStatusEnum.Pending;

        public CustomerEntity? Customer { get; set; }

        public CarEntity? Car { get; set; }

        public RentalEntity? Rental { get; set; }

        public int GetDurationInDays() => (EndDate - StartDate).Days;
        public bool IsActive() => BookingStatus == BookingStatusEnum.Confirmed && EndDate >= DateTime.Now;
    }
}
