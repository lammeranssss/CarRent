using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.API.Models.Requests.Bookings;
using CarRental.API.Models.Responses.Bookings;
using CarRental.API.Abstractions.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

public class BookingsController(IBookingService service, IMapper mapper)
    : CrudControllerBase<CreateBookingRequest, CreateBookingRequest, BookingResponse>
{
    private readonly IBookingService _service = service;
    private readonly IMapper _mapper = mapper;

    public override async Task<IEnumerable<BookingResponse>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _service.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<BookingResponse>>(items);
    }

    public override async Task<BookingResponse> GetById(Guid id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<BookingResponse>(item);
    }

    public override async Task<BookingResponse> Create([FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<BookingModel>(request);
        var created = await _service.AddAsync(model, cancellationToken);
        return _mapper.Map<BookingResponse>(created);
    }

    public override async Task<BookingResponse> Update(Guid id, [FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<BookingModel>(request);
        model.Id = id;
        var updated = await _service.UpdateAsync(model, cancellationToken);
        return _mapper.Map<BookingResponse>(updated);
    }

    public override async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.RemoveAsync(id, cancellationToken);
    }
}
