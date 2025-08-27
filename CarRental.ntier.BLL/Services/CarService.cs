using CarRental.ntier.BLL.Models;
using CarRental.ntier.BLL.Abstractions;
using CarRental.ntier.DAL.Models.Enums;

namespace CarRental.ntier.BLL.Services
{
    public class CarService : ICarService
    {
        public bool IsAvailableForRent(CarModel car, DateTime startDate, DateTime endDate)
        {
            return car.CarStatus == CarStatusEnum.Available &&
                   !car.Bookings.Any(b => b.StartDate <= endDate &&
                                          b.EndDate >= startDate &&
                                          (b.BookingStatus == BookingStatusEnum.Confirmed ||
                                           b.BookingStatus == BookingStatusEnum.Pending));
        }

        public decimal CalculateRentalPrice(CarModel car, int days)
        {
            return car.DailyRate * days;
        }

        public bool RequiresMaintenance(CarModel car)
        {
            return car.Mileage > 10000;
        }
    }
}
