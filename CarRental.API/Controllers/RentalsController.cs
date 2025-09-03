using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.API.Models.Requests.Rentals;
using CarRental.API.Models.Responses.Rentals;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalsController : ControllerBase
{
    private readonly IRentalService _service;
    private readonly IMapper _mapper;

    public RentalsController(IRentalService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RentalResponse>>> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(ct);
        return Ok(_mapper.Map<IEnumerable<RentalResponse>>(items));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RentalResponse>> GetById(Guid id, CancellationToken ct)
    {
        var model = await _service.GetByIdAsync(id, ct);
        if (model is null) return NotFound();
        return Ok(_mapper.Map<RentalResponse>(model));
    }

    [HttpPost]
    public async Task<ActionResult<RentalResponse>> Create(CreateRentalRequest request, CancellationToken ct)
    {
        var model = _mapper.Map<RentalModel>(request);
        var created = await _service.AddAsync(model, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<RentalResponse>(created));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RentalResponse>> Update(Guid id, UpdateRentalRequest request, CancellationToken ct)
    {
        var model = _mapper.Map<RentalModel>(request);
        model.Id = id;
        var updated = await _service.UpdateAsync(model, ct);
        return Ok(_mapper.Map<RentalResponse>(updated));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _service.RemoveAsync(id, ct);
        return NoContent();
    }
}
