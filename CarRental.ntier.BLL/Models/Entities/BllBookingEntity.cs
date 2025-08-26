using CarRental.ntier.BLL.Abstractions;
using CarRental.ntier.DAL.Models.Enums;

namespace CarRental.ntier.BLL.Models.Entities
{
    class BllBookingEntity : BaseModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CarId { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatusEnum BookingStatus { get; set; }

        public BllCustomerEntity Customer { get; set; }
        public BllCarEntity Car { get; set; }
        public BllRentalEntity Rental { get; set; }

        public bool IsActive()
        {
            return BookingStatus == BookingStatusEnum.Confirmed &&
                   StartDate <= DateTime.Now &&
                   EndDate >= DateTime.Now;
        }

        public int GetDurationInDays()
        {
            return (EndDate - StartDate).Days;
        }

        public bool CanBeCancelled()
        {
            return BookingStatus == BookingStatusEnum.Pending ||
                   BookingStatus == BookingStatusEnum.Confirmed;
        }
    }
}
