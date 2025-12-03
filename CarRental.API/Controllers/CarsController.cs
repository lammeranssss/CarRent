using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.API.Models.Requests.Cars;
using CarRental.API.Models.Responses.Cars;
using CarRental.API.Abstractions.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CarRental.API.Controllers;

[ApiController]
[Route(ApiRoutes.Cars.Base)]
[Authorize]
public class CarsController(ICarService service, IMapper mapper) : ControllerBase
{
    private readonly ICarService _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IEnumerable<CarResponse>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _service.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CarResponse>>(items);
    }

    [HttpGet(ApiRoutes.Id)]
    public async Task<CarResponse> GetById(Guid id, CancellationToken cancellationToken)
    {
        var model = await _service.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<CarResponse>(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<CarResponse> Create([FromBody] CreateCarRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<CarModel>(request);
        var created = await _service.AddAsync(model, cancellationToken);
        return _mapper.Map<CarResponse>(created);
    }

    [HttpPut(ApiRoutes.Id)]
    [Authorize(Roles = "Admin")]
    public async Task<CarResponse> Update(Guid id, [FromBody] CreateCarRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<CarModel>(request);
        model.Id = id;
        var updated = await _service.UpdateAsync(model, cancellationToken);
        return _mapper.Map<CarResponse>(updated);
    }

    [HttpDelete(ApiRoutes.Id)]
    [Authorize(Roles = "Admin")]
    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.RemoveAsync(id, cancellationToken);
    }
}
