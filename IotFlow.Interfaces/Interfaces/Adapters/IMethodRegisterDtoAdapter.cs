using IotFlow.Abstractions.Abstrations;

namespace IotFlow.Abstractions.Interfaces.Adapters
{
    public interface IMethodRegisterDtoAdapter<TMethodRegisterResultDto, TMethodRegister> where TMethodRegister : BaseEntity
    {
        Task<TMethodRegisterResultDto> GetMethodRegisterResultDto(TMethodRegister device);
    }
}
