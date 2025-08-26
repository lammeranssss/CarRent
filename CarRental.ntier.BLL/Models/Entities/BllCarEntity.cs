using CarRental.ntier.BLL.Abstractions;
using CarRental.ntier.DAL.Models.Enums;

namespace CarRental.ntier.BLL.Models.Entities
{
    class BllCarEntity : BaseModel
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public Guid LocationId { get; set; }
        public CarStatusEnum CarStatus { get; set; }
        public decimal DailyRate { get; set; }
        public decimal Mileage { get; set; }

        public BllLocationEntity Location { get; set; }
        public ICollection<BllBookingEntity> Bookings { get; set; } = new List<BllBookingEntity>();

        public bool IsAvailableForRent(DateTime startDate, DateTime endDate)
        {
            return CarStatus == CarStatusEnum.Available &&
                   !Bookings.Any(b => b.StartDate <= endDate && b.EndDate >= startDate &&
                                     (b.BookingStatus == BookingStatusEnum.Confirmed || b.BookingStatus == BookingStatusEnum.Pending));
        }

        public decimal CalculateRentalPrice(int days)
        {
            return DailyRate * days;
        }

        public bool RequiresMaintenance()
        {
            return Mileage > 10000; 
        }
    }
}