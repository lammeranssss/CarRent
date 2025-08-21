using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.ntier.DAL.Entities
{
    public class CustomerEntity
    {
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string LicenseNumber { get; set; }

        public ICollection<BookingEntity> Bookings { get; set; }
    }
}