using AutoMapper;
using IotFlow.Models.DB;
using IotFlow.Models.DTO.MethodRegister;

namespace IotFlow.Mappers
{
    public class MethodRegisterProfile : Profile
    {
        public MethodRegisterProfile()
        {
            CreateMap<MethodRegister, MethodRegisterResultDto>();
        }
    }
}
