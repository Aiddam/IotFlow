using IotFlow.Abstractions.Abstrations;

namespace IotFlow.Abstractions.Interfaces.DA
{
    public interface IUnitOfWork
    {
        Task<IRepository<T>> GetRepository<T>()
            where T : BaseEntity;
        Task CommitAsync();
    }
}
