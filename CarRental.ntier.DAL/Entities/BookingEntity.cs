using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.ntier.DAL.Entities
{
    public class BookingEntity
    {
        public Guid BookingId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CustomerId { get; set; } 
        public int CarId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } // e.g., "Pending", "Confirmed", "Completed"
    }
}