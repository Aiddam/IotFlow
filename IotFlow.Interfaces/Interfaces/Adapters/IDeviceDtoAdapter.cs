using IotFlow.Abstractions.Abstrations;

namespace IotFlow.Abstractions.Interfaces.Adapters
{
    public interface IDeviceDtoAdapter<TDeviceDto, TDevice> where TDeviceDto : IDeviceDto where TDevice : BaseEntity
    {
        Task<ICollection<TDeviceDto>> GetDeviceDtosAsync(ICollection<TDevice> devices);
        Task<TDeviceDto> GetDeviceDtoAsync(TDevice device);
    }
}
