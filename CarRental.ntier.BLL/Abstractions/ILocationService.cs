using CarRental.ntier.BLL.Models;

namespace CarRental.ntier.BLL.Abstractions
{
    public interface ILocationService
    {
        int GetAvailableCarsCount(LocationModel location);
        bool CanAcceptReturns(LocationModel location);
    }
}
