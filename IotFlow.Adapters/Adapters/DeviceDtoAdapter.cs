using AutoMapper;
using IotFlow.Abstractions.Interfaces.Adapters;
using IotFlow.Models.DB;
using IotFlow.Models.DTO.Devices;

namespace IotFlow.Adapters.Adapters
{
    public class DeviceDtoAdapter : IDeviceDtoAdapter<DeviceDto, DeviceWithIdDto, Device>
    {
        private readonly IMapper _mapper;
        public DeviceDtoAdapter(IMapper mapper)
        {
            _mapper = mapper;
        }
        public Task<DeviceDto> GetDeviceDtoAsync(Device device)
        {
            var mappedDevice = _mapper.Map<DeviceDto>(device);
            return Task.FromResult(mappedDevice);
        }

        public Task<ICollection<DeviceDto>> GetDeviceDtosAsync(ICollection<Device> devices)
        {
            ICollection<DeviceDto> mappedDevaices = new List<DeviceDto>();
            foreach (var device in devices)
            {
                var mappedDevice = _mapper.Map<DeviceDto>(device);
                mappedDevaices.Add(mappedDevice);
            }
            return Task.FromResult(mappedDevaices);
        }

        public Task<DeviceWithIdDto> GetDeviceWidhIdDtoAsync(Device device)
        {
            var mappedDevice = _mapper.Map<DeviceWithIdDto>(device);
            return Task.FromResult(mappedDevice);
        }
    }
}
