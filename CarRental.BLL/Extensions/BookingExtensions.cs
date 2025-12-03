using CarRental.BLL.Models;
using CarRental.DAL.Models.Enums;

namespace CarRental.BLL.Extensions;
public static class BookingExtensions
{
    public static bool IsActive(this BookingModel booking) =>
        booking.BookingStatus == BookingStatus.Confirmed &&
        booking.StartDate <= DateTime.UtcNow &&
        booking.EndDate >= DateTime.UtcNow;

    public static int GetDurationInDays(this BookingModel booking) =>
        (booking.EndDate - booking.StartDate).Days;

    public static bool CanBeCancelled(this BookingModel booking) =>
        booking.BookingStatus is BookingStatus.Pending or BookingStatus.Confirmed;
}
