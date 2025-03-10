﻿
using IotFlow.Models.DTO.Devices.Send;

namespace IotFlow.Models.DTO.Commands
{
    public class CommandDto
    {
        public string Command {  get; set; } = string.Empty;
        public string DeviceGuid { get; set; } = string.Empty;
        public ICollection<SendMethodParameterDto>? Parameters { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
