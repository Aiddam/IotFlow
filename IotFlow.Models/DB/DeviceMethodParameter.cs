using IotFlow.Abstractions.Abstrations;
using IotFlow.Models.Enum;

namespace IotFlow.Models.DB
{
    public class DeviceMethodParameter : BaseEntity
    {
        public string ParameterName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public ParameterType ParameterType { get; set; }
        public int Order { get; set; }
        public string? DefaultValue { get; set; }
        public int DeviceMethodId { get; set; }
        public DeviceMethod DeviceMethod { get; set; }
    }
}
