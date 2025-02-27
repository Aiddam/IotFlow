using AutoMapper;
using IotFlow.Models.DB;
using IotFlow.Models.DTO.Devices;

namespace IotFlow.Mappers
{
    public class DeviceMapperProfile : Profile
    {
        public DeviceMapperProfile()
        {
            CreateMap<Device, DeviceDto>();
            CreateMap<Device, DeviceWithIdDto>();
        }
    }
}
