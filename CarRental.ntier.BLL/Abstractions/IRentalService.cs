using CarRental.ntier.BLL.Models;

namespace CarRental.ntier.BLL.Abstractions
{
    public interface IRentalService
    {
        bool IsActive(RentalModel rental);
        decimal CalculateMileageUsed(RentalModel rental);
        bool HasExceededMileageLimit(RentalModel rental, decimal dailyLimit = 200);
    }
}