using CarRental.ntier.BLL.Abstractions;

namespace CarRental.ntier.BLL.Models.Entities
{
    class BllCustomerEntity : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string LicenseNumber { get; set; }

        public ICollection<BllBookingEntity> Bookings { get; set; } = new List<BllBookingEntity>();

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        public bool HasValidLicense()
        {
            return !string.IsNullOrEmpty(LicenseNumber) && LicenseNumber.Length >= 8;
        }
    }
}
