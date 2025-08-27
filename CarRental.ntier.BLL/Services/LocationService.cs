using CarRental.ntier.BLL.Models;
using CarRental.ntier.BLL.Abstractions;
using CarRental.ntier.DAL.Models.Enums;

namespace CarRental.ntier.BLL.Services
{
    public class LocationService : ILocationService
    {
        public int GetAvailableCarsCount(LocationModel location)
        {
            return location.Cars.Count(c => c.CarStatus == CarStatusEnum.Available);
        }

        public bool CanAcceptReturns(LocationModel location)
        {
            return true;
        }
    }
}
