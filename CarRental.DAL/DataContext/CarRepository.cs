using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;

namespace CarRental.DAL.DataContext;

public class CarRepository(CarRentalDbContext context) : GenericRepository<CarEntity>(context), ICarRepository
{
}
