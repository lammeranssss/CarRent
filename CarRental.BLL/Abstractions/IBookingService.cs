using CarRental.BLL.Models;
using CarRental.DAL.Models.Entities;

namespace CarRental.BLL.Abstractions;

public interface IBookingService : IGenericService<BookingModel, BookingEntity>
{
    Task<BookingModel> ConfirmBookingAsync(Guid bookingId, CancellationToken cancellationToken = default);
    Task<BookingModel> CancelBookingAsync(Guid bookingId, string? reason, CancellationToken cancellationToken = default);
}