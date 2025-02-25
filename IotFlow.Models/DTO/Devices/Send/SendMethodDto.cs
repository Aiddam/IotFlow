
namespace IotFlow.Models.DTO.Devices.Send
{
    public class SendMethodDto
    {
        public string MethodName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public List<SendMethodParameterDto> Parameters { get; set; } = new List<SendMethodParameterDto>();
        public string Description { get; set; } = string.Empty;
    }
}
