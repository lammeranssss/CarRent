using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;
using CarRental.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.DAL.DataContext;
public class CarRentalDbContext(DbContextOptions<CarRentalDbContext> options)
    : DbContext(options)
{
    public static async Task ApplyMigrationsAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
        if (context.Database.IsRelational())
        {
            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(
                async () => await context.Database.MigrateAsync()
            );
        }
    }

    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<CarEntity> Cars { get; set; }
    public DbSet<LocationEntity> Locations { get; set; }
    public DbSet<BookingEntity> Bookings { get; set; }
    public DbSet<RentalEntity> Rentals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresEnum<BookingStatusEnum>();
        modelBuilder.HasPostgresEnum<CarStatusEnum>();

        modelBuilder.Entity<CarEntity>()
            .Property(c => c.CarStatus)
            .HasDefaultValue(CarStatusEnum.Unknown);

        modelBuilder.Entity<CarEntity>()
            .HasIndex(c => c.LicensePlate)
            .IsUnique();

        modelBuilder.Entity<BookingEntity>()
            .Property(b => b.BookingStatus)
            .HasDefaultValue(BookingStatusEnum.Unknown);

        modelBuilder.Entity<BookingEntity>()
            .HasOne(b => b.Customer)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookingEntity>()
            .HasOne(b => b.Car)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RentalEntity>()
            .HasOne(r => r.Booking)
            .WithOne(b => b.Rental)
            .HasForeignKey<RentalEntity>(r => r.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RentalEntity>()
            .HasOne(r => r.PickUpLocation)
            .WithMany(l => l.PickUpRentals)
            .HasForeignKey(r => r.PickUpLocationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RentalEntity>()
            .HasOne(r => r.DropOffLocation)
            .WithMany(l => l.DropOffRentals)
            .HasForeignKey(r => r.DropOffLocationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LocationEntity>()
            .HasMany(l => l.Cars)
            .WithOne(c => c.Location)
            .HasForeignKey(c => c.LocationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CustomerEntity>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<CustomerEntity>()
            .HasIndex(c => c.LicenseNumber)
            .IsUnique();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity &&
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
