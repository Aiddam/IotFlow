using IotFlow.Abstractions.Abstrations;

namespace IotFlow.Models.DB
{
    public class DeviceMethod : BaseEntity
    {
        public string MethodName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public ICollection<DeviceMethodParameter> Parameters { get; set; } = new List<DeviceMethodParameter>();
        public int DeviceId { get; set; }
        public required Device Device { get; set; }
    }
}
