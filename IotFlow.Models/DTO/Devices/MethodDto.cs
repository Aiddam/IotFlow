
namespace IotFlow.Models.DTO.Devices
{
    public class MethodDto
    {
        public string MethodName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public List<MethodParametrDto> Parameters { get; set; } = new List<MethodParametrDto>();
        public string Description { get; set; } = string.Empty;
    }
}
