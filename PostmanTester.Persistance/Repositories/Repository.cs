using Microsoft.EntityFrameworkCore;
using PostmanTester.Application.Interfaces.Repositories;
using PostmanTester.Application.Models.Enums;
using PostmanTester.Domain.Entities;
using PostmanTester.Persistance.Contexts;
using System.Linq.Expressions;

namespace PostmanTester.Persistance.Repositories
{
    public class Repository<T, TId> : IRepository<T, TId> where T : BaseEntity<TId>, new()
    {
        protected readonly PostmanTesterDbContext _context;
        private DbSet<T> _entities
        {
            get => _context.Set<T>();
        }

        public Repository(PostmanTesterDbContext context)
        {
            _context = context;
        }

        private IQueryable<T> IncludeTables(IQueryable<T> listItem, params Expression<Func<T, object>>[] includeTables)
        {
            foreach (var includeTable in includeTables)
            {
                listItem = listItem.Include(includeTable);
            }
            return listItem;
        }

        public async Task<T> AddAsync(T entity)
        {
            entity.Status = (byte)GeneralEnum.Active;
            entity.CreatedDate = DateTime.UtcNow;
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task AddRangeAsync(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.Status = (byte)GeneralEnum.Active;
                entity.CreatedDate = DateTime.UtcNow;
            }
            await _entities.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<T> DeleteAsync(T entity)
        {
            entity.Status = (byte)GeneralEnum.Deleted;
            entity.DeletedDate = DateTime.UtcNow;
            _entities.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteRangeAsync(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.Status = (byte)GeneralEnum.Deleted;
                entity.DeletedDate = DateTime.UtcNow;
            }
            _entities.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> expression, bool isTracking = true, params Expression<Func<T, object>>[] includeTables)
        {
            var listItem = isTracking ? _entities : _entities.AsNoTracking();
            listItem = IncludeTables(listItem, includeTables);
            return await listItem.FirstOrDefaultAsync(expression);
        }

        public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>>? expression = null, bool isTracking = true, params Expression<Func<T, object>>[] includeTables)
        {
            var listItem = isTracking ? _entities : _entities.AsNoTracking();
            listItem = IncludeTables(listItem, includeTables);
            return expression == null 
                ? await listItem.ToListAsync() 
                : await listItem.Where(expression).ToListAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(ICollection<T> entities)
        {
            _entities.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            _entities.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateRangeAsync(ICollection<T> entities)
        {
            foreach (var entity in entities)
                entity.UpdatedDate = DateTime.UtcNow;
            _entities.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
