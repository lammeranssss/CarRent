namespace CarRental.BLL.Abstractions;

public interface IGenericService<TModel>
{
    Task<TModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TModel> AddAsync(TModel model, CancellationToken cancellationToken = default);
    Task<TModel> UpdateAsync(TModel model, CancellationToken cancellationToken = default);
    Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
}
