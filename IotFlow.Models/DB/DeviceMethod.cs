using IotFlow.Abstractions.Abstrations;
using IotFlow.Models.Enum;

namespace IotFlow.Models.DB
{
    public class DeviceMethod : BaseEntity
    {
        public string MethodName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public MethodType MethodType { get; set; } = MethodType.Void;
        public ICollection<DeviceMethodParameter> Parameters { get; set; } = new List<DeviceMethodParameter>();
        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
}
