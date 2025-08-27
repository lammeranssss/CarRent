using CarRental.ntier.BLL.Abstractions;

namespace CarRental.ntier.BLL.Models
{
    public class CustomerModel : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string LicenseNumber { get; set; }

        public ICollection<BookingModel> Bookings { get; set; } = [];
    }
}
