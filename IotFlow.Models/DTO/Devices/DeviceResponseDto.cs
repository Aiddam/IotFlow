using IotFlow.Models.DB;

namespace IotFlow.Models.DTO.Devices
{
    public class DeviceResponseDto : IDeviceDto
    {
        public Guid DeviceGuid { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsAlive { get; set; } = false;
        public DateTime? LastSeen { get; set; }
        public ICollection<DeviceMethod> Methods { get; set; } = new List<DeviceMethod>();
    }
}
