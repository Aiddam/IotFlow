﻿
namespace IotFlow.Models.DTO.Devices
{
    public class MethodDto
    {
        public string MethodName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public List<MethodParameterDto> Parameters { get; set; } = new List<MethodParameterDto>();
        public string Description { get; set; } = string.Empty;
    }
}
