using CarRental.ntier.DAL.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.ntier.DAL.Models.Entities
{
    [Table("Rentals")]
    public class RentalEntity : BaseEntity
    {

        public Guid BookingId { get; set; }

        public DateTime PickUpDate { get; set; }

        public DateTime DropOffDate { get; set; }

        public Guid PickUpLocationId { get; set; }

        public Guid DropOffLocationId { get; set; }

        public decimal InitialMileage { get; set; }

        public decimal FinalMileage { get; set; }

        public decimal FinalPrice { get; set; }

        public BookingEntity? Booking { get; set; }

        public LocationEntity? PickUpLocation { get; set; }

        public LocationEntity? DropOffLocation { get; set; }

        public decimal CalculateMileageUsed() => FinalMileage - InitialMileage;
    }
}
