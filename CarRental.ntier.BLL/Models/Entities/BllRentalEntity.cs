using CarRental.ntier.BLL.Abstractions;

namespace CarRental.ntier.BLL.Models.Entities
{
    class BllRentalEntity : BaseModel
    {
        public Guid BookingId { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public Guid PickUpLocationId { get; set; }
        public Guid DropOffLocationId { get; set; }
        public decimal InitialMileage { get; set; }
        public decimal FinalMileage { get; set; }
        public decimal FinalPrice { get; set; }

        public BllBookingEntity Booking { get; set; }
        public BllLocationEntity PickUpLocation { get; set; }
        public BllLocationEntity DropOffLocation { get; set; }

        public bool IsActive()
        {
            return PickUpDate <= DateTime.Now && DropOffDate >= DateTime.Now;
        }

        public decimal CalculateMileageUsed()
        {
            return FinalMileage - InitialMileage;
        }

        public bool HasExceededMileageLimit(decimal dailyLimit = 200)
        {
            if (FinalMileage == 0) return false;

            var daysRented = (DropOffDate - PickUpDate).Days;
            var totalAllowedMileage = daysRented * dailyLimit;
            return CalculateMileageUsed() > totalAllowedMileage;
        }
    }
}
