
namespace IotFlow.Models.DTO.Devices
{
    public class HandleDeviceResponseDto
    {
        public Guid CorrelationId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Result { get; set; }
    }
}
