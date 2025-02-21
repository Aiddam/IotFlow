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
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }
    }
}
