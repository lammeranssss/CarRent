using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;
using CarRental.Messaging; 
using CarRental.Messaging.Events; 
using CarRental.Utilities.Abstractions; 

namespace CarRental.BLL.Services;

public class CustomerService(
    ICustomerRepository repository,
    IMapper mapper,
    IEventSender eventSender,
    ITraceIdProvider traceIdProvider,
    IDateTimeProvider dateTimeProvider) : GenericService<CustomerModel, CustomerEntity>(repository, mapper), ICustomerService
{
    public override async Task<CustomerModel> AddAsync(CustomerModel model, CancellationToken cancellationToken = default)
    {
        var newCustomerModel = await base.AddAsync(model, cancellationToken);

        var customerEvent = _mapper.Map<CustomerRegisteredEvent>(newCustomerModel);

        var wrappedEvent = new EventWrapper<CustomerRegisteredEvent>
        {
            Payload = customerEvent,
            TraceId = traceIdProvider.GetTraceId(),
            Timestamp = dateTimeProvider.CurrentDateTime
        };

        await eventSender.SendAsync(wrappedEvent, cancellationToken);
        return newCustomerModel;
    }
}
