using IotFlow.Abstractions.Abstrations;

namespace IotFlow.Models.DB
{
    public class DeviceMethodParameter : BaseEntity
    {
        public string ParameterName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string ParameterType { get; set; } = string.Empty;
        public int Order { get; set; }
        public string? DefaultValue { get; set; }
        public int DeviceMethodId { get; set; }
        public required DeviceMethod DeviceMethod { get; set; }
    }
}
