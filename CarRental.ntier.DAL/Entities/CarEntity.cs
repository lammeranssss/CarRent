using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.ntier.DAL.Entities
{
    public class CarEntity
    {
        public Guid CarId { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public int LocationId { get; set; } // Foreign key to Location
        public string Status { get; set; } // e.g., "Available", "Booked", "Maintenance"
        public decimal DailyRate { get; set; }

        public LocationEntity Location { get; set; }
        public ICollection<BookingEntity> Bookings { get; set; }
    }
}