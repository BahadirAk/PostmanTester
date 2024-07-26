using PostmanTester.Domain.Entities;
using System.Linq.Expressions;

namespace PostmanTester.Application.Interfaces.Repositories
{
    public interface IRepository<T, TId> where T : BaseEntity<TId>, new()
    {
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);
        Task RemoveAsync(T entity);
        Task<T?> GetAsync(Expression<Func<T, bool>> expression, bool isTracking = true, params Expression<Func<T, object>>[] includeTables);
        Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>>? expression = null, bool isTracking = true, params Expression<Func<T, object>>[] includeTables);
        Task AddRangeAsync(ICollection<T> entities);
        Task UpdateRangeAsync(ICollection<T> entities);
        Task DeleteRangeAsync(ICollection<T> entities);
        Task RemoveRangeAsync(ICollection<T> entities);
    }
}
