using IotFlow.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IotFlow.DataAccess.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(DbContext context) : base(context) { }

        public override async Task<IEnumerable<User>> ReadEntitiesByPredicate(Expression<Func<User, bool>> predicate, IEnumerable<KeyValuePair<Expression<Func<User, object>>, bool>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .Where(predicate);

            if (orderBy is not null)
            {
                foreach (var order in orderBy)
                {
                    query = order.Value ? query.OrderBy(order.Key) : query.OrderByDescending(order.Key);
                }
            }

            return await query.ToListAsync(cancellationToken);
        }

        public override async Task<User?> ReadEntityByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }    }
}
