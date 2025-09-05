using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.API.Models.Requests.Locations;
using CarRental.API.Models.Responses.Locations;
using CarRental.API.Abstractions.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

public class LocationsController(
    ILocationService _service,
    IMapper _mapper
) : CrudControllerBase<CreateLocationRequest, CreateLocationRequest, LocationResponse>
{
    public override async Task<IEnumerable<LocationResponse>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _service.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<LocationResponse>>(items);
    }

    public override async Task<LocationResponse> GetById(Guid id, CancellationToken cancellationToken)
    {
        var model = await _service.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<LocationResponse>(model);
    }

    public override async Task<LocationResponse> Create([FromBody] CreateLocationRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<LocationModel>(request);
        var created = await _service.AddAsync(model, cancellationToken);
        return _mapper.Map<LocationResponse>(created);
    }

    public override async Task<LocationResponse> Update(Guid id, [FromBody] CreateLocationRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<LocationModel>(request);
        model.Id = id;
        var updated = await _service.UpdateAsync(model, cancellationToken);
        return _mapper.Map<LocationResponse>(updated);
    }

    public override async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.RemoveAsync(id, cancellationToken);
    }
}
