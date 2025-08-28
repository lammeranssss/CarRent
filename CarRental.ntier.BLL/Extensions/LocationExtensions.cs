using CarRental.DAL.Models.Enums;
using CarRental.BLL.Models;

namespace CarRental.BLL.Extensions;
public static class LocationExtensions
{
    public static int GetAvailableCarsCount(this LocationModel location) =>
        location.Cars.Count(c => c.CarStatus == CarStatusEnum.Available);

    public static bool CanAcceptReturns(this LocationModel location) => true;
}
