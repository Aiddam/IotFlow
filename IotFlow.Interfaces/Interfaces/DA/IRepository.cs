using IotFlow.Abstractions.Abstrations;
using System.Linq.Expressions;

namespace IotFlow.Abstractions.Interfaces.DA
{
    public interface IRepository<T>
        where T : BaseEntity
    {
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<T?> ReadEntityByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> ReadEntitiesByPredicate(Expression<Func<T, bool>> predicate, IEnumerable<KeyValuePair<Expression<Func<T, object>>, bool>>? orderBy = null, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
