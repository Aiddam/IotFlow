using IotFlow.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IotFlow.DataAccess.Repositories
{
    public class MethodRegisterRepository : BaseRepository<MethodRegister>
    {
        public MethodRegisterRepository(DbContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<MethodRegister>> ReadEntitiesByPredicate(Expression<Func<MethodRegister, bool>> predicate, IEnumerable<KeyValuePair<Expression<Func<MethodRegister, object>>, bool>>? orderBy = null,
    CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .Include(dev => dev.Method)
                    .ThenInclude(dev => dev.Parameters)
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
    }
}
