using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;
using CarRental.Messaging;
using CarRental.Messaging.Events;
using CarRental.Utilities.Abstractions;
using CarRental.BLL.Extensions;
using CarRental.DAL.Models.Enums;
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
    public override async Task<RentalModel> AddAsync(RentalModel model, CancellationToken cancellationToken = default)
    {
        var newRentalModel = await base.AddAsync(model, cancellationToken);

        var booking = await bookingRepository.GetByIdAsync(newRentalModel.BookingId, cancellationToken);
        var car = (booking is not null) ? await carRepository.GetByIdAsync(booking.CarId, cancellationToken) : null;
        var customer = (booking is not null) ? await customerRepository.GetByIdAsync(booking.CustomerId, cancellationToken) : null;

        var rentalStartedEvent = _mapper.Map<RentalStartedEvent>(newRentalModel, opts =>
        {
            if (customer is not null) opts.Items["Customer"] = customer;
            if (car is not null) opts.Items["Car"] = car;
        });

        await PublishEventAsync(rentalStartedEvent, cancellationToken);

        return newRentalModel;
    }

    public override async Task<RentalModel> UpdateAsync(RentalModel model, CancellationToken cancellationToken = default)
    {
        var modelId = model.Id;
        var existingEntity = await repository.GetByIdWithNoTrackingAsync(modelId, cancellationToken);

        mapper.Map(model, existingEntity);

        await _repository.UpdateAsync(existingEntity, cancellationToken);

        var updatedRentalModel = _mapper.Map<RentalModel>(existingEntity);

        var booking = await bookingRepository.GetByIdAsync(updatedRentalModel.BookingId, cancellationToken);
        var car = (booking is not null) ? await carRepository.GetByIdAsync(booking.CarId, cancellationToken) : null;
        var customer = (booking is not null) ? await customerRepository.GetByIdAsync(booking.CustomerId, cancellationToken) : null;
        var kilometersDriven = updatedRentalModel.CalculateMileageUsed();

        if (car is not null)    
        {
            var carModel = _mapper.Map<CarModel>(car);

            carModel.Mileage = updatedRentalModel.FinalMileage;

            carModel.CarStatus = carModel.RequiresMaintenance() ? CarStatus.Maintenance : CarStatus.Available;

            _mapper.Map(carModel, car);
            await carRepository.UpdateAsync(car, cancellationToken);
        }

        var rentalCompletedEvent = _mapper.Map<RentalCompletedEvent>(updatedRentalModel, opts =>
        {
            if (customer is not null) opts.Items["Customer"] = customer;
            if (car is not null) opts.Items["Car"] = car;
        });

        await PublishEventAsync(rentalCompletedEvent, cancellationToken);

        return updatedRentalModel;
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
