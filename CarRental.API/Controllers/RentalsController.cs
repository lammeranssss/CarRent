using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.API.Models.Requests.Rentals;
using CarRental.API.Models.Responses.Rentals;
using CarRental.API.Abstractions.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

public class RentalsController(
    IRentalService _service,
    IMapper _mapper
) : CrudControllerBase<CreateRentalRequest, UpdateRentalRequest, RentalResponse>
{
    public override async Task<IEnumerable<RentalResponse>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _service.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<RentalResponse>>(items);
    }

    public override async Task<RentalResponse> GetById(Guid id, CancellationToken cancellationToken)
    {
        var model = await _service.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<RentalResponse>(model);
    }

    public override async Task<RentalResponse> Create([FromBody] CreateRentalRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<RentalModel>(request);
        var created = await _service.AddAsync(model, cancellationToken);
        return _mapper.Map<RentalResponse>(created);
    }

    public override async Task<RentalResponse> Update(Guid id, [FromBody] UpdateRentalRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<RentalModel>(request);
        model.Id = id;
        var updated = await _service.UpdateAsync(model, cancellationToken);
        return _mapper.Map<RentalResponse>(updated);
    }

    public override async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.RemoveAsync(id, cancellationToken);
    }
}
