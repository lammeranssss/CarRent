using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;

namespace CarRental.DAL.DataContext;

public class BookingRepository(CarRentalDbContext context) : GenericRepository<BookingEntity>(context), IBookingRepository
{
}
