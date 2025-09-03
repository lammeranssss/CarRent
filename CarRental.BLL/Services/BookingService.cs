using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;

namespace CarRental.BLL.Services;

public class BookingService(IBookingRepository repository, IMapper mapper) : GenericService<BookingModel, BookingEntity>(repository, mapper), IBookingService
{
    private readonly IBookingRepository _repository = repository;
}
