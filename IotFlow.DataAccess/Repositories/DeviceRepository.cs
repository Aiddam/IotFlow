using IotFlow.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IotFlow.DataAccess.Repositories
{
    public class DeviceRepository : BaseRepository<Device>
    {
        public DeviceRepository(DbContext context) : base(context)
        {
        }
        public override async Task<Device?> ReadEntityByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(dev => dev.Methods)
                    .ThenInclude(dev => dev.Parameters)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }
        public override async Task<IEnumerable<Device>> ReadEntitiesByPredicate(Expression<Func<Device, bool>> predicate, IEnumerable<KeyValuePair<Expression<Func<Device, object>>, bool>>? orderBy = null,
    CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .Include(dev => dev.Methods)
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
