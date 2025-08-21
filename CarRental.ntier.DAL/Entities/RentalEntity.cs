using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.ntier.DAL.Entities
{
    public class RentalEntity
    {
        public Guid RentalId { get; set; }
        public int BookingId { get; set; } // Foreign key to Booking
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public int PickUpLocationId { get; set; } // Foreign key to Location
        public int DropOffLocationId { get; set; } // Foreign key to Location
        public decimal InitialMileage { get; set; }
        public decimal FinalMileage { get; set; }
        public decimal FinalPrice { get; set; } // Might differ from Booking.TotalPrice
    }
}