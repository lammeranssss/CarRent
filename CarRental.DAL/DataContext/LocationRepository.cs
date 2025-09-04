using CarRental.DAL.Models.Entities;

using CarRental.DAL.Abstractions;

namespace CarRental.DAL.DataContext;

public class LocationRepository(CarRentalDbContext context) : GenericRepository<LocationEntity>(context), ILocationRepository
{
}
