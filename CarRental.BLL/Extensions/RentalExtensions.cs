using CarRental.BLL.Models;

namespace CarRental.BLL.Extensions;
public static class RentalExtensions
{
    public static bool IsActive(this RentalModel rental) =>
        rental.PickUpDate <= DateTime.UtcNow && rental.DropOffDate >= DateTime.UtcNow;

    public static decimal CalculateMileageUsed(this RentalModel rental) =>
        rental.FinalMileage - rental.InitialMileage;

    public static bool HasExceededMileageLimit(this RentalModel rental, decimal dailyLimit = 200)
    {
        if (rental.FinalMileage == 0) return false;

        var daysRented = (rental.DropOffDate - rental.PickUpDate).Days;
        var totalAllowedMileage = daysRented * dailyLimit;
        return rental.CalculateMileageUsed() > totalAllowedMileage;
    }
}
