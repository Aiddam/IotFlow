﻿namespace IotFlow.Models.DTO.Devices.Update
{
    public class UpdateDeviceDto : IDeviceDto
    {
        public Guid DeviceGuid { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
