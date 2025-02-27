using AutoMapper;
using IotFlow.Abstractions.Interfaces.Adapters;
using IotFlow.Models.DB;
using IotFlow.Models.DTO.Devices;
using IotFlow.Models.DTO.MethodRegister;

namespace IotFlow.Adapters.Adapters
{
    public class MethodRegisterDtoAdapter : IMethodRegisterDtoAdapter<MethodRegisterResultDto, MethodRegister>
    {
        private readonly IMapper _mapper;
        public MethodRegisterDtoAdapter(IMapper mapper)
        {
            _mapper = mapper;
        }
        public Task<MethodRegisterResultDto> GetMethodRegisterResultDto(MethodRegister device)
        {
            var mappedResult = _mapper.Map<MethodRegisterResultDto>(device);
            return Task.FromResult(mappedResult);
        }
    }
}
