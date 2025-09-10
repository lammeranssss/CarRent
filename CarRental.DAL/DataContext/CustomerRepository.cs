using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;

namespace CarRental.DAL.DataContext;

public class CustomerRepository(CarRentalDbContext context) : GenericRepository<CustomerEntity>(context), ICustomerRepository
{
}
