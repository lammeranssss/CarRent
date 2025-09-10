using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;

namespace CarRental.DAL.DataContext;

public class RentalRepository(CarRentalDbContext context) : GenericRepository<RentalEntity>(context), IRentalRepository
{
}
