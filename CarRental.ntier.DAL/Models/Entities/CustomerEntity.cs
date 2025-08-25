using CarRental.ntier.DAL.Abstractions;

namespace CarRental.ntier.DAL.Models.Entities
{
    public class CustomerEntity : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string LicenseNumber { get; set; }

        public ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
    }
}
