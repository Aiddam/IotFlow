
namespace IotFlow.Abstractions.Interfaces.Services
{
    public interface IIotFlowApiService<TCommandRequest>
    {
        Task SendCommandAsync(TCommandRequest commandRequest, CancellationToken cancellationToken = default);
    }
}
