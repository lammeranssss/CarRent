using CarRental.DAL.Models.Enums;
using CarRental.BLL.Models;

namespace CarRental.BLL.Extensions;
public static class CarExtensions
{
    public static bool IsAvailableForRent(this CarModel car, DateTime startDate, DateTime endDate) =>
        car.CarStatus == CarStatusEnum.Available &&
        !car.Bookings.Any(b => b.StartDate <= endDate &&
                               b.EndDate >= startDate &&
                               (b.BookingStatus == BookingStatusEnum.Confirmed ||
                                b.BookingStatus == BookingStatusEnum.Pending));

    public static decimal CalculateRentalPrice(this CarModel car, int days) =>
        car.DailyRate * days;

    public static bool RequiresMaintenance(this CarModel car) =>
        car.Mileage > 10000;
}
