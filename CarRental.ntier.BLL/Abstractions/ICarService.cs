using CarRental.ntier.BLL.Models;

namespace CarRental.ntier.BLL.Abstractions
{
    public interface ICarService
    {
        bool IsAvailableForRent(CarModel car, DateTime startDate, DateTime endDate);
        decimal CalculateRentalPrice(CarModel car, int days);
        bool RequiresMaintenance(CarModel car);
    }
}
