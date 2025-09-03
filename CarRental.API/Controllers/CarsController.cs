using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.API.Models.Requests.Cars;
using CarRental.API.Models.Responses.Cars;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ICarService _service;
    private readonly IMapper _mapper;

    public CarsController(ICarService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarResponse>>> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(ct);
        return Ok(_mapper.Map<IEnumerable<CarResponse>>(items));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CarResponse>> GetById(Guid id, CancellationToken ct)
    {
        var model = await _service.GetByIdAsync(id, ct);
        if (model is null) return NotFound();
        return Ok(_mapper.Map<CarResponse>(model));
    }

    [HttpPost]
    public async Task<ActionResult<CarResponse>> Create(CreateCarRequest request, CancellationToken ct)
    {
        var model = _mapper.Map<CarModel>(request);
        var created = await _service.AddAsync(model, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<CarResponse>(created));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CarResponse>> Update(Guid id, CreateCarRequest request, CancellationToken ct)
    {
        var model = _mapper.Map<CarModel>(request);
        model.Id = id;
        var updated = await _service.UpdateAsync(model, ct);
        return Ok(_mapper.Map<CarResponse>(updated));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _service.RemoveAsync(id, ct);
        return NoContent();
    }
}
