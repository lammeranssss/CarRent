using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.API.Models.Requests.Locations;
using CarRental.API.Models.Responses.Locations;
using CarRental.API.Abstractions.Routing;
using Microsoft.AspNetCore.Mvc;
using CarRental.API.Models.Responses.Cars;
using CarRental.BLL.Exceptions;

namespace CarRental.API.Controllers;

[ApiController]
[Route(ApiRoutes.Locations.Base)]
public class LocationsController(ILocationService service, IMapper mapper) : ControllerBase
{
    private readonly ILocationService _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IEnumerable<LocationResponse>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _service.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<LocationResponse>>(items);
    }

    [HttpGet(ApiRoutes.Id)]
    public async Task<LocationResponse> GetById(Guid id, CancellationToken cancellationToken)
    {
        var model = await _service.GetByIdAsync(id, cancellationToken);
        return model is null ? throw new NotFoundException($"model with {id} is not found") : _mapper.Map<LocationResponse>(model);
    }

    [HttpPost]
    public async Task<LocationResponse> Create([FromBody] CreateLocationRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<LocationModel>(request);
        var created = await _service.AddAsync(model, cancellationToken);
        return _mapper.Map<LocationResponse>(created);
    }

    [HttpPut(ApiRoutes.Id)]
    public async Task<LocationResponse> Update(Guid id, [FromBody] CreateLocationRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<LocationModel>(request);
        model.Id = id;
        var updated = await _service.UpdateAsync(model, cancellationToken);
        return _mapper.Map<LocationResponse>(updated);
    }

    [HttpDelete(ApiRoutes.Id)]
    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.RemoveAsync(id, cancellationToken);
    }
}
