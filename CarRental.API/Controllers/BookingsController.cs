using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.API.Models.Requests.Bookings;
using CarRental.API.Models.Responses.Bookings;
using CarRental.API.Abstractions.Routing;
using Microsoft.AspNetCore.Mvc;
using CarRental.BLL.Exceptions;

namespace CarRental.API.Controllers;

[ApiController]
[Route(ApiRoutes.Bookings.Base)]
public class BookingsController(IBookingService service, IMapper mapper) : ControllerBase
{
    private readonly IBookingService _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IEnumerable<BookingResponse>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _service.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<BookingResponse>>(items);
    }

    [HttpGet(ApiRoutes.Id)]
    public async Task<BookingResponse> GetById(Guid id, CancellationToken cancellationToken)
    {
        var model = await _service.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<BookingResponse>(model);
    }

    [HttpPost]
    public async Task<BookingResponse> Create([FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<BookingModel>(request);
        var created = await _service.AddAsync(model, cancellationToken);
        return _mapper.Map<BookingResponse>(created);
    }

    [HttpPut(ApiRoutes.Id)]
    public async Task<BookingResponse> Update(Guid id, [FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<BookingModel>(request);
        model.Id = id;
        var updated = await _service.UpdateAsync(model, cancellationToken);
        return _mapper.Map<BookingResponse>(updated);
    }

    [HttpDelete(ApiRoutes.Id)]
    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.RemoveAsync(id, cancellationToken);
    }
}
