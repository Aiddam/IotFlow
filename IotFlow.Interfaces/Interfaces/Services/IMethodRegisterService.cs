
namespace IotFlow.Abstractions.Interfaces.Services
{
    public interface IMethodRegisterService<THandleDeviceResponseDto, TMethodRegisterResultDto>
    {
        public Task<Guid> CreateCommandRegister(int userId, Guid deviceGuid, string methodName, CancellationToken cancellationToken = default);
        public Task UpdateMethodResponseAsync(THandleDeviceResponseDto responseDto, CancellationToken cancellationToken = default);
        public Task<TMethodRegisterResultDto> GetFirstMethodRegisterByDeviceAsync(int userId, Guid deviceGuid, string methodName, CancellationToken cancellationToken = default);

    }
}
