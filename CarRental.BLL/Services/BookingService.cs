using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.DAL.Abstractions;
using CarRental.DAL.DataContext;
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
        model.Status = BookingStatus.Pending;
        var newBookingModel = await base.AddAsync(model, cancellationToken);

        var car = await carRepository.GetByIdAsync(newBookingModel.CarId, cancellationToken);
        var customer = await customerRepository.GetByIdAsync(newBookingModel.CustomerId, cancellationToken);

        var bookingEvent = _mapper.Map<BookingCreatedEvent>(newBookingModel, opts =>
        {
            if (customer is not null) opts.Items["Customer"] = customer;
            if (car is not null) opts.Items["Car"] = car;
        });

        await PublishEventAsync(bookingEvent, cancellationToken);

        return newBookingModel;
    }

    public async Task<BookingModel> ConfirmBookingAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var bookingEntity = await _repository.GetByIdAsync(bookingId, cancellationToken) ?? throw new KeyNotFoundException("Booking not found");
        bookingEntity.BookingStatus = BookingStatus.Confirmed;
        var updatedEntity = await _repository.UpdateAsync(bookingEntity, cancellationToken);

        var customer = await customerRepository.GetByIdAsync(updatedEntity.CustomerId, cancellationToken);
        var car = await carRepository.GetByIdAsync(updatedEntity.CarId, cancellationToken);

        if (car is not null)
        {
            var carModel = _mapper.Map<CarModel>(car);
            carModel.CarStatus = CarStatus.Booked;
            _mapper.Map(carModel, car);
            await carRepository.UpdateAsync(car, cancellationToken);
        }

        var confirmedEvent = _mapper.Map<BookingConfirmedEvent>(updatedEntity, opts =>
        {
            if (customer is not null) opts.Items["Customer"] = customer;
            if (car is not null) opts.Items["Car"] = car;
        });

        await PublishEventAsync(confirmedEvent, cancellationToken);
        return _mapper.Map<BookingModel>(updatedEntity);
    }

    public async Task<BookingModel> CancelBookingAsync(Guid bookingId, string? reason, CancellationToken cancellationToken = default)
    {
        var bookingEntity = await _repository.GetByIdAsync(bookingId, cancellationToken) ?? throw new KeyNotFoundException("Booking not found");
        bookingEntity.BookingStatus = BookingStatus.Cancelled;
        var updatedEntity = await _repository.UpdateAsync(bookingEntity, cancellationToken);

        var customer = await customerRepository.GetByIdAsync(bookingEntity.CustomerId, cancellationToken);
        var car = await carRepository.GetByIdAsync(bookingEntity.CarId, cancellationToken);

        if (car is not null)
        {
            var carModel = _mapper.Map<CarModel>(car);
            carModel.CarStatus = CarStatus.Available;
            _mapper.Map(carModel, car);
            await carRepository.UpdateAsync(car, cancellationToken);
        }

        var cancelledEvent = _mapper.Map<BookingCancelledEvent>(updatedEntity, opts =>
        {
            if (customer is not null) opts.Items["Customer"] = customer;
            if (car is not null) opts.Items["Car"] = car;
        });

        cancelledEvent.Reason = reason;

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
