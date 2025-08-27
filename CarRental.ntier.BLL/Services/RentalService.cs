using CarRental.ntier.BLL.Models;
using CarRental.ntier.BLL.Abstractions;

namespace CarRental.ntier.BLL.Services
{
    public class RentalService : IRentalService
    {
        public bool IsActive(RentalModel rental)
        {
            return rental.PickUpDate <= DateTime.UtcNow &&
                   rental.DropOffDate >= DateTime.UtcNow;
        }

        public decimal CalculateMileageUsed(RentalModel rental)
        {
            return rental.FinalMileage - rental.InitialMileage;
        }

        public bool HasExceededMileageLimit(RentalModel rental, decimal dailyLimit = 200)
        {
            if (rental.FinalMileage == 0) return false;

            var daysRented = (rental.DropOffDate - rental.PickUpDate).Days;
            var totalAllowedMileage = daysRented * dailyLimit;
            return CalculateMileageUsed(rental) > totalAllowedMileage;
        }
    }
}
