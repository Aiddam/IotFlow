
namespace IotFlow.Abstractions.Interfaces.Services
{
    public interface IDeviceService<TDeviceDto, TRegisterDeviceDto, TUpdateDeviceDto> where TDeviceDto : IDeviceDto where TRegisterDeviceDto : IDeviceDto
    {
        public Task<TDeviceDto> CreateDevice(TRegisterDeviceDto registerDevice, CancellationToken cancellationToken = default);
        public Task<TDeviceDto> UpdateDevice(TUpdateDeviceDto updateDevice, CancellationToken cancellationToken = default);
        public Task DeleteDevice(Guid deviceGuid, int userId, CancellationToken cancellationToken = default);
        public Task<TDeviceDto> GetDeviceByGuid(Guid deviceGuid, int userId, CancellationToken cancellationToken = default);
        public Task<ICollection<TDeviceDto>> GetDevices(int userId, CancellationToken cancellationToken = default);
    }
}
public interface IDeviceDto
{
}