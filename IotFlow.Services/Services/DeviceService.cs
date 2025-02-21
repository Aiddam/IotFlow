using IotFlow.Abstractions.Interfaces.Adapters;
using IotFlow.Abstractions.Interfaces.DA;
using IotFlow.Abstractions.Interfaces.Services;
using IotFlow.Models.DB;
using IotFlow.Models.DTO.Devices;

namespace IotFlow.Services.Services
{
    public class DeviceService : IDeviceService<DeviceDto, RegisterDeviceDto, UpdateDeviceDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDeviceDtoAdapter<DeviceDto, Device> _adapter;

        public DeviceService(IUnitOfWork unitOfWork, IDeviceDtoAdapter<DeviceDto, Device> adapter)
        {
            _unitOfWork = unitOfWork;
            _adapter = adapter;
        }

        public async Task<DeviceDto> CreateDevice(RegisterDeviceDto registerDevice, CancellationToken cancellationToken = default)
        {
            Device newDevice = new Device
            {
                Name = registerDevice.Name,
                UserId = registerDevice.UserId,
                IsAlive = false
            };

            return await ProcessDeviceAsync(newDevice, cancellationToken: cancellationToken);
        }
        public async Task<DeviceDto> UpdateDevice(UpdateDeviceDto updateDevice, CancellationToken cancellationToken = default)
        {
            var deviceRepository = await _unitOfWork.GetRepository<Device>();

            var foundedDevices = await deviceRepository.ReadEntitiesByPredicate(
                device => device.DeviceGuid == updateDevice.DeviceGuid && device.UserId == updateDevice.UserId);

            Device? existingDevice = foundedDevices.FirstOrDefault();

            if (existingDevice == null)
            {
                throw new ArgumentException("Device not found");
            }

            existingDevice.Name = updateDevice.Name;

            return await ProcessDeviceAsync(existingDevice, cancellationToken);
        }

        public async Task DeleteDevice(Guid deviceGuid, int userId, CancellationToken cancellationToken = default)
        {
            var deviceRepository = await _unitOfWork.GetRepository<Device>();
            var foundedDevices = await deviceRepository.ReadEntitiesByPredicate(device => device.DeviceGuid == deviceGuid && device.UserId == userId);
            var firstDevice = foundedDevices.FirstOrDefault();
            if(firstDevice == null)
            {
                throw new ArgumentException("Unable to delete device");
            }
            await deviceRepository.DeleteAsync(firstDevice.Id);
        }

        public async Task<DeviceDto> GetDeviceByGuid(Guid deviceGuid, int userId, CancellationToken cancellationToken = default)
        {
            var deviceRepository = await _unitOfWork.GetRepository<Device>();
            var foundedDevices = await deviceRepository.ReadEntitiesByPredicate(device => deviceGuid == device.DeviceGuid && device.UserId == userId);
            Device? firstDevice = foundedDevices.FirstOrDefault();
            if (firstDevice == null)
            {
                throw new ArgumentException("Unable to read device");
            }
            return await _adapter.GetDeviceDtoAsync(firstDevice);
        }
        public async Task<ICollection<DeviceDto>> GetDevices(int userId, CancellationToken cancellationToken = default)
        {
            var deviceRepository = await _unitOfWork.GetRepository<Device>();
            var foundedDevices = await deviceRepository.ReadEntitiesByPredicate(device => device.UserId == userId);
            return await _adapter.GetDeviceDtosAsync((ICollection<Device>)foundedDevices);
        }
        public async Task<DeviceDto> ProcessDeviceAsync(Device device, CancellationToken cancellationToken = default)
        {
            var deviceRepository = await _unitOfWork.GetRepository<Device>();

            if (device.Id == 0)
            {
                await deviceRepository.CreateAsync(device, cancellationToken: cancellationToken);
            }
            else
            {
                await deviceRepository.UpdateAsync(device, cancellationToken: cancellationToken);
            }
            await _unitOfWork.CommitAsync();

            return new DeviceDto()
            {
                DeviceGuid = device.DeviceGuid,
                Name = device.Name,
                IsAlive = device.IsAlive,
                Methods = device.Methods,
                LastSeen = device.LastSeen
            };
        }

    }
}
