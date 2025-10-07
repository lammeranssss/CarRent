using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.API.Models.Requests.Customers;
using CarRental.API.Models.Responses.Customers;
using CarRental.API.Abstractions.Routing;
using Microsoft.AspNetCore.Mvc;
using CarRental.API.Models.Responses.Cars;
using CarRental.BLL.Exceptions;

namespace CarRental.API.Controllers;

[ApiController]
[Route(ApiRoutes.Customers.Base)]
public class CustomersController(ICustomerService service, IMapper mapper) : ControllerBase
{
    private readonly ICustomerService _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IEnumerable<CustomerResponse>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _service.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CustomerResponse>>(items);
    }

    [HttpGet(ApiRoutes.Id)]
    public async Task<CustomerResponse> GetById(Guid id, CancellationToken cancellationToken)
    {
        var model = await _service.GetByIdAsync(id, cancellationToken);
        return model is null ? throw new NotFoundException($"model with {id} is not found") : _mapper.Map<CustomerResponse>(model);
    }

    [HttpPost]
    public async Task<CustomerResponse> Create([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<CustomerModel>(request);
        var created = await _service.AddAsync(model, cancellationToken);
        return _mapper.Map<CustomerResponse>(created);
    }

    [HttpPut(ApiRoutes.Id)]
    public async Task<CustomerResponse> Update(Guid id, [FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<CustomerModel>(request);
        model.Id = id;
        var updated = await _service.UpdateAsync(model, cancellationToken);
        return _mapper.Map<CustomerResponse>(updated);
    }

    [HttpDelete(ApiRoutes.Id)]
    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.RemoveAsync(id, cancellationToken);
    }
}
