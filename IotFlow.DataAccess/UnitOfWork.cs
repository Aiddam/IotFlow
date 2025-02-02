using IotFlow.Abstractions.Abstrations;
using IotFlow.Abstractions.Interfaces.DA;
using IotFlow.DataAccess.Repositories;

namespace IotFlow.DataAccess
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ServerContext _context;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(ServerContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }
        public Task<IRepository<T>> GetRepository<T>() where T : BaseEntity
        {
            var type = typeof(T);
            if (_repositories.ContainsKey(type)) return Task.FromResult((IRepository<T>)_repositories[type]);

            var repositoryType = typeof(UnitOfWork)
                                     .Assembly
                                     .GetTypes()
                                     .FirstOrDefault(t => t.IsAssignableTo(typeof(IRepository<T>)))
                                 ??
                                 typeof(BaseRepository<T>);
            var repositoryInstance = Activator.CreateInstance(repositoryType, _context)
                                     ?? throw new ArgumentException($"Unable to resolve repository with type {typeof(T).Name}");
            _repositories[type] = repositoryInstance;
            return Task.FromResult((IRepository<T>)_repositories[type]);
        }
        public Task CommitAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
