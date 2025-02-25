
namespace IotFlow.Abstractions.Interfaces.Services
{
    public interface IDeviceService<TDeviceDto, TRegisterDeviceDto, TUpdateDeviceDto, TMethodDto, TDeviceAliveDto, TSendParameterDto> where TDeviceDto : IDeviceDto where TRegisterDeviceDto : IDeviceDto
    {
        public Task<TDeviceDto> CreateDevice(TRegisterDeviceDto registerDevice, CancellationToken cancellationToken = default);
        public Task<TDeviceDto> UpdateDevice(TUpdateDeviceDto updateDevice, CancellationToken cancellationToken = default);
        public Task DeleteDevice(Guid deviceGuid, int userId, CancellationToken cancellationToken = default);
        public Task<TDeviceDto> GetDeviceByGuid(Guid deviceGuid, int userId, CancellationToken cancellationToken = default);
        public Task<ICollection<TDeviceDto>> GetDevices(int userId, CancellationToken cancellationToken = default);
        public Task AddMethods(Guid deviceGuid, int userId, List<TMethodDto> newMethods, CancellationToken cancellationToken = default);
        public Task SendCommandAsync(Guid deviceGuid, int userId, string command, ICollection<TSendParameterDto> parameters, CancellationToken cancellationToken = default);
        public Task<TDeviceDto> SetDeviceAlive(Guid deviceGuid, int userId, TDeviceAliveDto deviceAliveDto, CancellationToken cancellationToken = default);
    }
}
public interface IDeviceDto
{
}