using Microsoft.EntityFrameworkCore;

namespace CarRental.ntier.DAL.DataContext
{
    public class CarRentalDbContext : DbContext
    {
        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options)
            : base(options)
        {
        }

        public DbSet<Entities.BookingEntity> Bookings { get; set; }
        public DbSet<Entities.CarEntity> Cars { get; set; }
        public DbSet<Entities.CustomerEntity> Customers { get; set; }
        public DbSet<Entities.LocationEntity> Locations { get; set; }
        public DbSet<Entities.RentalEntity> Rentals { get; set; }
    }
}