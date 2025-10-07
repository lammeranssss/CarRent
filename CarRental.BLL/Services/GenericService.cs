using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.DAL.Abstractions;

namespace CarRental.BLL.Services;

public class GenericService<TModel, TEntity> : IGenericService<TModel, TEntity>
    where TEntity : BaseEntity
{
    private readonly IGenericRepository<TEntity> _repository;
    private readonly IMapper _mapper;

    public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdWithNoTrackingAsync(id, cancellationToken);
        return _mapper.Map<TModel>(entity);
    }

    public async Task<IReadOnlyList<TModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<TModel>>(entities);
    }

    public async Task<TModel> AddAsync(TModel model, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<TEntity>(model);
        var added = await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<TModel>(added);
    }

    public async Task<TModel> UpdateAsync(TModel model, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<TEntity>(model);
        var updated = await _repository.UpdateAsync(entity, cancellationToken);
        return _mapper.Map<TModel>(updated);
    }

    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException($"{typeof(TEntity).Name} with id {id} not found");
        await _repository.RemoveAsync(entity, cancellationToken);
    }
}
