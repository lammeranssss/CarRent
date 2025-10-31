using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;
using CarRental.DAL.Models.Enums;
using CarRental.Messaging;
using CarRental.Messaging.Events;
using CarRental.Utilities.Abstractions;

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
    private readonly IEventSender _eventSender = eventSender;
    private readonly ITraceIdProvider _traceIdProvider = traceIdProvider;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly ICarRepository _carRepository = carRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public override async Task<BookingModel> AddAsync(BookingModel model, CancellationToken cancellationToken = default)
    {
        model.BookingStatus = BookingStatusEnum.Pending;
        var newBookingModel = await base.AddAsync(model, cancellationToken);

        var car = await _carRepository.GetByIdAsync(newBookingModel.CarId, cancellationToken);
        var customer = await _customerRepository.GetByIdAsync(newBookingModel.CustomerId, cancellationToken);

        var bookingEvent = new BookingCreatedEvent
        {
            BookingId = newBookingModel.Id,
            CustomerEmail = customer?.Email,
            CustomerFirstName = customer?.FirstName,
            CarModel = (car != null) ? $"{car.Brand} {car.Model}" : null,
            StartDate = newBookingModel.StartDate,
            EndDate = newBookingModel.EndDate,
            TotalPrice = newBookingModel.TotalPrice
        };

        var wrappedEvent = new EventWrapper<BookingCreatedEvent>
        {
            Payload = bookingEvent,
            TraceId = _traceIdProvider.GetTraceId(),
            Timestamp = _dateTimeProvider.CurrentDateTime
        };

        await _eventSender.SendAsync(wrappedEvent, cancellationToken);
        return newBookingModel;
    }

    public async Task<BookingModel> ConfirmBookingAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var bookingEntity = await _repository.GetByIdAsync(bookingId, cancellationToken) ?? throw new KeyNotFoundException("Booking not found");
        bookingEntity.BookingStatus = BookingStatusEnum.Confirmed;
        var updatedEntity = await _repository.UpdateAsync(bookingEntity, cancellationToken);

        var customer = await _customerRepository.GetByIdAsync(bookingEntity.CustomerId, cancellationToken);
        var car = await _carRepository.GetByIdAsync(bookingEntity.CarId, cancellationToken);

        var confirmedEvent = new BookingConfirmedEvent
        {
            BookingId = bookingEntity.Id,
            CustomerEmail = customer?.Email,
            CustomerFirstName = customer?.FirstName,
            CarModel = (car != null) ? $"{car.Brand} {car.Model}" : null,
            StartDate = bookingEntity.StartDate
        };

        var wrappedEvent = new EventWrapper<BookingConfirmedEvent>
        {
            Payload = confirmedEvent,
            TraceId = _traceIdProvider.GetTraceId(),
            Timestamp = _dateTimeProvider.CurrentDateTime
        };
        await _eventSender.SendAsync(wrappedEvent, cancellationToken);

        return _mapper.Map<BookingModel>(updatedEntity);
    }

    public async Task<BookingModel> CancelBookingAsync(Guid bookingId, string? reason, CancellationToken cancellationToken = default)
    {
        var bookingEntity = await _repository.GetByIdAsync(bookingId, cancellationToken) ?? throw new KeyNotFoundException("Booking not found");
        bookingEntity.BookingStatus = BookingStatusEnum.Cancelled;
        var updatedEntity = await _repository.UpdateAsync(bookingEntity, cancellationToken);

        var customer = await _customerRepository.GetByIdAsync(bookingEntity.CustomerId, cancellationToken);
        var car = await _carRepository.GetByIdAsync(bookingEntity.CarId, cancellationToken);

        var cancelledEvent = new BookingCancelledEvent
        {
            BookingId = bookingEntity.Id,
            CustomerEmail = customer?.Email,
            CustomerFirstName = customer?.FirstName,
            CarModel = (car != null) ? $"{car.Brand} {car.Model}" : null,
            Reason = reason
        };

        var wrappedEvent = new EventWrapper<BookingCancelledEvent>
        {
            Payload = cancelledEvent,
            TraceId = _traceIdProvider.GetTraceId(),
            Timestamp = _dateTimeProvider.CurrentDateTime
        };
        await _eventSender.SendAsync(wrappedEvent, cancellationToken);

        return _mapper.Map<BookingModel>(updatedEntity);
    }
}
