using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.API.Models.Requests.Bookings;
using CarRental.API.Models.Responses.Bookings;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _service;
    private readonly IMapper _mapper;

    public BookingsController(IBookingService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingResponse>>> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(ct);
        return Ok(_mapper.Map<IEnumerable<BookingResponse>>(items));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookingResponse>> GetById(Guid id, CancellationToken ct)
    {
        var model = await _service.GetByIdAsync(id, ct);
        if (model is null) return NotFound();
        return Ok(_mapper.Map<BookingResponse>(model));
    }

    [HttpPost]
    public async Task<ActionResult<BookingResponse>> Create(CreateBookingRequest request, CancellationToken ct)
    {
        var model = _mapper.Map<BookingModel>(request);
        var created = await _service.AddAsync(model, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<BookingResponse>(created));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BookingResponse>> Update(Guid id, CreateBookingRequest request, CancellationToken ct)
    {
        var model = _mapper.Map<BookingModel>(request);
        model.Id = id;
        var updated = await _service.UpdateAsync(model, ct);
        return Ok(_mapper.Map<BookingResponse>(updated));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _service.RemoveAsync(id, ct);
        return NoContent();
    }
}
