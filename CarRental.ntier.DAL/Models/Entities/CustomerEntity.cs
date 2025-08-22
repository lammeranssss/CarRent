using CarRental.ntier.DAL.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.ntier.DAL.Models.Entities
{
    [Table("Customers")]
    public class CustomerEntity : BaseEntity
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string LicenseNumber { get; set; }

        // Навигационные свойства
        public ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();

        public string GetFullName() => $"{FirstName} {LastName}";
    }
}