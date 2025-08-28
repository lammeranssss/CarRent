using CarRental.ntier.BLL.Models;
using CarRental.ntier.DAL.Models.Enums;

namespace CarRental.ntier.BLL.Extensions;
public static class BookingExtensions
{
    public static bool IsActive(this BookingModel booking) =>
        booking.BookingStatus == BookingStatusEnum.Confirmed &&
        booking.StartDate <= DateTime.UtcNow &&
        booking.EndDate >= DateTime.UtcNow;

    public static int GetDurationInDays(this BookingModel booking) =>
        (booking.EndDate - booking.StartDate).Days;

    public static bool CanBeCancelled(this BookingModel booking) =>
        booking.BookingStatus is BookingStatusEnum.Pending or BookingStatusEnum.Confirmed;
}
