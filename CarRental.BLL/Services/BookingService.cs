using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;
using CarRental.DAL.Models.Enums;
using CarRental.Messaging;
using CarRental.Messaging.Events;
using CarRental.Utilities.Abstractions;
using CarRental.Utilities.Infrastructure;

namespace CarRental.BLL.Services;

public class BookingService(
    IBookingRepository repository,
    IMapper mapper,
    IEventSender eventSender,
    ITraceIdProvider traceIdProvider,
    IDateTimeProvider dateTimeProvider,
    ICarRepository carRepository,
    ICustomerRepository customerRepository) : GenericService<BookingModel, BookingEntity>(repository, mapper), IBookingService
{
    public override async Task<BookingModel> AddAsync(BookingModel model, CancellationToken cancellationToken = default)
    {
        model.BookingStatus = BookingStatusEnum.Pending;
        var newBookingModel = await base.AddAsync(model, cancellationToken);

        var car = await carRepository.GetByIdAsync(newBookingModel.CarId, cancellationToken);
        var customer = await customerRepository.GetByIdAsync(newBookingModel.CustomerId, cancellationToken);

        var bookingEvent = new BookingCreatedEvent
        {
            BookingId = newBookingModel.Id,
            CustomerEmail = customer?.Email,
            CustomerFirstName = customer?.FirstName,
            CarModel = (car is not null) ? $"{car.Brand} {car.Model}" : null,
            StartDate = newBookingModel.StartDate,
            EndDate = newBookingModel.EndDate,
            TotalPrice = newBookingModel.TotalPrice
        };

        await PublishEventAsync(bookingEvent, cancellationToken);

        return newBookingModel;
    }

    public async Task<BookingModel> ConfirmBookingAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var bookingEntity = await _repository.GetByIdAsync(bookingId, cancellationToken) ?? throw new KeyNotFoundException("Booking not found");
        bookingEntity.BookingStatus = BookingStatusEnum.Confirmed;
        var updatedEntity = await _repository.UpdateAsync(bookingEntity, cancellationToken);

        var customer = await customerRepository.GetByIdAsync(bookingEntity.CustomerId, cancellationToken);
        var car = await carRepository.GetByIdAsync(bookingEntity.CarId, cancellationToken);

        var confirmedEvent = new BookingConfirmedEvent
        {
            BookingId = bookingEntity.Id,
            CustomerEmail = customer?.Email,
            CustomerFirstName = customer?.FirstName,
            CarModel = (car is not null) ? $"{car.Brand} {car.Model}" : null,
            StartDate = bookingEntity.StartDate
        };

        await PublishEventAsync(confirmedEvent, cancellationToken);

        return _mapper.Map<BookingModel>(updatedEntity);
    }

    public async Task<BookingModel> CancelBookingAsync(Guid bookingId, string? reason, CancellationToken cancellationToken = default)
    {
        var bookingEntity = await _repository.GetByIdAsync(bookingId, cancellationToken) ?? throw new KeyNotFoundException("Booking not found");
        bookingEntity.BookingStatus = BookingStatusEnum.Cancelled;
        var updatedEntity = await _repository.UpdateAsync(bookingEntity, cancellationToken);

        var customer = await customerRepository.GetByIdAsync(bookingEntity.CustomerId, cancellationToken);
        var car = await carRepository.GetByIdAsync(bookingEntity.CarId, cancellationToken);

        var cancelledEvent = new BookingCancelledEvent
        {
            BookingId = bookingEntity.Id,
            CustomerEmail = customer?.Email,
            CustomerFirstName = customer?.FirstName,
            CarModel = (car is not null) ? $"{car.Brand} {car.Model}" : null,
            Reason = reason
        };

        await PublishEventAsync(cancelledEvent, cancellationToken);

        return _mapper.Map<BookingModel>(updatedEntity);
    }

    private async Task PublishEventAsync<T>(T payload, CancellationToken cancellationToken = default) where T : class
    {
        var wrappedEvent = new EventWrapper<T>
        {
            Payload = payload,
            TraceId = traceIdProvider.GetTraceId(),
            Timestamp = dateTimeProvider.CurrentDateTime
        };

        await eventSender.SendAsync(wrappedEvent, cancellationToken);
    }
}
