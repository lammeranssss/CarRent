using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;
using CarRental.Messaging;
using CarRental.Messaging.Events;
using CarRental.Utilities.Abstractions;
using CarRental.BLL.Extensions; // <-- Для CalculateMileageUsed

namespace CarRental.BLL.Services;

public class RentalService(
    IRentalRepository repository,
    IMapper mapper,
    IEventSender eventSender,
    ITraceIdProvider traceIdProvider,
    IDateTimeProvider dateTimeProvider,
    IBookingRepository bookingRepository,
    ICarRepository carRepository,
    ICustomerRepository customerRepository) : GenericService<RentalModel, RentalEntity>(repository, mapper), IRentalService
{
    private readonly IEventSender _eventSender = eventSender;
    private readonly ITraceIdProvider _traceIdProvider = traceIdProvider;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IBookingRepository _bookingRepository = bookingRepository;
    private readonly ICarRepository _carRepository = carRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public override async Task<RentalModel> AddAsync(RentalModel model, CancellationToken cancellationToken = default)
    {
        var newRentalModel = await base.AddAsync(model, cancellationToken);

        var booking = await _bookingRepository.GetByIdAsync(newRentalModel.BookingId, cancellationToken);
        var car = (booking != null) ? await _carRepository.GetByIdAsync(booking.CarId, cancellationToken) : null;
        var customer = (booking != null) ? await _customerRepository.GetByIdAsync(booking.CustomerId, cancellationToken) : null;

        var rentalStartedEvent = new RentalStartedEvent
        {
            RentalId = newRentalModel.Id,
            BookingId = newRentalModel.BookingId,
            CustomerEmail = customer?.Email,
            CustomerFirstName = customer?.FirstName,
            CarModel = (car != null) ? $"{car.Brand} {car.Model}" : null,
            CarLicensePlate = car?.LicensePlate,
            PickUpDate = newRentalModel.PickUpDate,
            InitialMileage = newRentalModel.InitialMileage
        };

        var wrappedEvent = new EventWrapper<RentalStartedEvent>
        {
            Payload = rentalStartedEvent,
            TraceId = _traceIdProvider.GetTraceId(),
            Timestamp = _dateTimeProvider.CurrentDateTime
        };
        await _eventSender.SendAsync(wrappedEvent, cancellationToken);

        return newRentalModel;
    }

    public override async Task<RentalModel> UpdateAsync(RentalModel model, CancellationToken cancellationToken = default)
    {
        var modelId = model.Id;
        var existingEntity = await _repository.GetByIdAsync(modelId, cancellationToken);

        existingEntity.DropOffDate = model.DropOffDate;
        existingEntity.FinalMileage = model.FinalMileage;
        existingEntity.FinalPrice = model.FinalPrice;
        existingEntity.PickUpDate = model.PickUpDate;
        existingEntity.PickUpLocationId = model.PickUpLocationId;
        existingEntity.DropOffLocationId = model.DropOffLocationId;
        existingEntity.InitialMileage = model.InitialMileage;

        await _repository.UpdateAsync(existingEntity, cancellationToken);
        var updatedRentalModel = _mapper.Map<RentalModel>(existingEntity);

        var booking = await _bookingRepository.GetByIdAsync(updatedRentalModel.BookingId, cancellationToken);
        var car = (booking != null) ? await _carRepository.GetByIdAsync(booking.CarId, cancellationToken) : null;
        var customer = (booking != null) ? await _customerRepository.GetByIdAsync(booking.CustomerId, cancellationToken) : null;
        var kilometersDriven = updatedRentalModel.CalculateMileageUsed();

        var rentalCompletedEvent = new RentalCompletedEvent
        {
            RentalId = updatedRentalModel.Id,
            BookingId = updatedRentalModel.BookingId,
            CustomerEmail = customer?.Email,
            CarModel = (car != null) ? $"{car.Brand} {car.Model}" : null,
            DropOffDate = updatedRentalModel.DropOffDate,
            FinalMileage = updatedRentalModel.FinalMileage,
            InitialMileage = updatedRentalModel.InitialMileage,
            FinalPrice = updatedRentalModel.FinalPrice,
            KilometersDriven = kilometersDriven
        };

        var wrappedEvent = new EventWrapper<RentalCompletedEvent>
        {
            Payload = rentalCompletedEvent,
            TraceId = _traceIdProvider.GetTraceId(),
            Timestamp = _dateTimeProvider.CurrentDateTime
        };
        await _eventSender.SendAsync(wrappedEvent, cancellationToken);

        return updatedRentalModel;
    }
}