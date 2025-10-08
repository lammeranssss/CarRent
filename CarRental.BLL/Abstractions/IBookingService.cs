using CarRental.BLL.Models;
using CarRental.DAL.Models.Entities;

namespace CarRental.BLL.Abstractions;

public interface IBookingService : IGenericService<BookingModel, BookingEntity> { }
