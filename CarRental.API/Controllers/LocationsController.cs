using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.API.Models.Requests.Locations;
using CarRental.API.Models.Responses.Locations;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILocationService _service;
    private readonly IMapper _mapper;

    public LocationsController(ILocationService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LocationResponse>>> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(ct);
        return Ok(_mapper.Map<IEnumerable<LocationResponse>>(items));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LocationResponse>> GetById(Guid id, CancellationToken ct)
    {
        var model = await _service.GetByIdAsync(id, ct);
        if (model is null) return NotFound();
        return Ok(_mapper.Map<LocationResponse>(model));
    }

    [HttpPost]
    public async Task<ActionResult<LocationResponse>> Create(CreateLocationRequest request, CancellationToken ct)
    {
        var model = _mapper.Map<LocationModel>(request);
        var created = await _service.AddAsync(model, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<LocationResponse>(created));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<LocationResponse>> Update(Guid id, CreateLocationRequest request, CancellationToken ct)
    {
        var model = _mapper.Map<LocationModel>(request);
        model.Id = id;
        var updated = await _service.UpdateAsync(model, ct);
        return Ok(_mapper.Map<LocationResponse>(updated));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _service.RemoveAsync(id, ct);
        return NoContent();
    }
}
