using CarRental.ntier.BLL.Models;

namespace CarRental.ntier.BLL.Abstractions
{
    public interface IBookingService
    {
        bool IsActive(BookingModel booking);
        int GetDurationInDays(BookingModel booking);
        bool CanBeCancelled(BookingModel booking);
    }
}