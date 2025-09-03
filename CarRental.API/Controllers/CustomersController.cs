using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.API.Models.Requests.Customers;
using CarRental.API.Models.Responses.Customers;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _service;
    private readonly IMapper _mapper;

    public CustomersController(ICustomerService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(ct);
        return Ok(_mapper.Map<IEnumerable<CustomerResponse>>(items));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerResponse>> GetById(Guid id, CancellationToken ct)
    {
        var model = await _service.GetByIdAsync(id, ct);
        if (model is null) return NotFound();
        return Ok(_mapper.Map<CustomerResponse>(model));
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create(CreateCustomerRequest request, CancellationToken ct)
    {
        var model = _mapper.Map<CustomerModel>(request);
        var created = await _service.AddAsync(model, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<CustomerResponse>(created));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CustomerResponse>> Update(Guid id, CreateCustomerRequest request, CancellationToken ct)
    {
        var model = _mapper.Map<CustomerModel>(request);
        model.Id = id;
        var updated = await _service.UpdateAsync(model, ct);
        return Ok(_mapper.Map<CustomerResponse>(updated));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _service.RemoveAsync(id, ct);
        return NoContent();
    }
}
