using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.API.Models.Requests.Cars;
using CarRental.API.Models.Responses.Cars;
using CarRental.API.Abstractions.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

public class CarsController(ICarService service, IMapper mapper)
    : CrudControllerBase<CreateCarRequest, CreateCarRequest, CarResponse>
{
    private readonly ICarService _service = service;
    private readonly IMapper _mapper = mapper;

    public override async Task<IEnumerable<CarResponse>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _service.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CarResponse>>(items);
    }

    public override async Task<CarResponse> GetById(Guid id, CancellationToken cancellationToken)
    {
        var model = await _service.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<CarResponse>(model);
    }

    public override async Task<CarResponse> Create([FromBody] CreateCarRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<CarModel>(request);
        var created = await _service.AddAsync(model, cancellationToken);
        return _mapper.Map<CarResponse>(created);
    }

    public override async Task<CarResponse> Update(Guid id, [FromBody] CreateCarRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<CarModel>(request);
        model.Id = id;
        var updated = await _service.UpdateAsync(model, cancellationToken);
        return _mapper.Map<CarResponse>(updated);
    }

    public override async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.RemoveAsync(id, cancellationToken);
    }
}
