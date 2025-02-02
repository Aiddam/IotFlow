using IotFlow.Abstractions.Abstrations;
using IotFlow.Abstractions.Interfaces.DA;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IotFlow.DataAccess.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var result = await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }

        public virtual async Task<T?> ReadEntityByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> ReadEntitiesByPredicate(Expression<Func<T, bool>> predicate, IEnumerable<KeyValuePair<Expression<Func<T, object>>, bool>>? orderBy = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.Where(predicate);

            if (orderBy is not null)
            {
                foreach (var order in orderBy)
                {
                    query = order.Value ? query.OrderBy(order.Key) : query.OrderByDescending(order.Key);
                }
            }

            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var oldEntity = await ReadEntityByIdAsync(entity.Id, cancellationToken);

            if (oldEntity is null) return entity;

            var oldEntityEntry = _context.Entry(oldEntity)!;
            oldEntityEntry.CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken);
            if (entity == null)
            {
                return false;
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
