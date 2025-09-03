using CarRental.DAL.Models.Entities;

namespace CarRental.DAL.DataContext;

public class LocationRepository(CarRentalDbContext context) : GenericRepository<LocationEntity>(context), ILocationRepository
{
}
