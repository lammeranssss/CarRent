using CarRental.ntier.DAL.Abstractions;
using CarRental.ntier.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarRental.ntier.DAL.DataContext;
public class GenericRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly CarRentalDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(CarRentalDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id) =>
        await _dbSet.FindAsync(id);

    public async Task<IReadOnlyList<T>> GetAllAsync() =>
        await _dbSet.AsNoTracking().ToListAsync();

    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.AsNoTracking().Where(predicate).ToListAsync();

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public Task<int> SaveChangesAsync() =>
        _context.SaveChangesAsync();
}
