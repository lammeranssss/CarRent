using CarRental.API.Abstractions.Routing;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Abstractions.Controllers;

[ApiController]
[Route(ApiRoutes.Base)]
public abstract class CrudControllerBase<TCreateRequest, TUpdateRequest, TResponse> : ControllerBase
{
    [HttpGet]
    public abstract Task<IEnumerable<TResponse>> GetAll(CancellationToken cancellationToken);

    [HttpGet(ApiRoutes.GetById)]
    public abstract Task<TResponse> GetById(Guid id, CancellationToken cancellationToken);

    [HttpPost]
    public abstract Task<TResponse> Create([FromBody] TCreateRequest request, CancellationToken cancellationToken);

    [HttpPut(ApiRoutes.Update)]
    public abstract Task<TResponse> Update(Guid id, [FromBody] TUpdateRequest request, CancellationToken cancellationToken);

    [HttpDelete(ApiRoutes.Delete)]
    public abstract Task Delete(Guid id, CancellationToken cancellationToken);
}
