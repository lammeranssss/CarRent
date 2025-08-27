using CarRental.ntier.BLL.Models;
using CarRental.ntier.BLL.Abstractions;
using CarRental.ntier.DAL.Models.Enums;

namespace CarRental.ntier.BLL.Services
{
    public class BookingService : IBookingService
    {
        public bool IsActive(BookingModel booking)
        {
            return booking.BookingStatus == BookingStatusEnum.Confirmed &&
                   booking.StartDate <= DateTime.UtcNow &&
                   booking.EndDate >= DateTime.UtcNow;
        }

        public int GetDurationInDays(BookingModel booking)
        {
            return (booking.EndDate - booking.StartDate).Days;
        }

        public bool CanBeCancelled(BookingModel booking)
        {
            return booking.BookingStatus == BookingStatusEnum.Pending ||
                   booking.BookingStatus == BookingStatusEnum.Confirmed;
        }
    }
}
