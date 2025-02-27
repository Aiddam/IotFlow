using IotFlow.Abstractions.Interfaces.Adapters;
using IotFlow.Abstractions.Interfaces.DA;
using IotFlow.Abstractions.Interfaces.Services;
using IotFlow.Models.DB;
using IotFlow.Models.DTO.Devices;
using IotFlow.Models.DTO.Devices.Register;
using IotFlow.Models.DTO.Devices.Send;
using IotFlow.Models.DTO.Devices.Update;
using IotFlow.Models.DTO.MethodRegister;
using IotFlow.Models.Enum;
using System.Linq.Expressions;

namespace IotFlow.Services.Services
{
    public class MethodRegisterService : IMethodRegisterService<HandleDeviceResponseDto, MethodRegisterResultDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDeviceService<DeviceDto, DeviceWithIdDto, RegisterDeviceDto, UpdateDeviceDto, MethodDto, DeviceAliveDto, SendMethodParameterDto> _deviceService;
        private readonly IMethodRegisterDtoAdapter<MethodRegisterResultDto, MethodRegister> _adapter;

        public MethodRegisterService(IUnitOfWork unitOfWork, IDeviceService<DeviceDto, DeviceWithIdDto, RegisterDeviceDto, UpdateDeviceDto, MethodDto, DeviceAliveDto, SendMethodParameterDto> deviceService, IMethodRegisterDtoAdapter<MethodRegisterResultDto, MethodRegister> adapter)
        {
            _unitOfWork = unitOfWork;
            _deviceService = deviceService;
            _adapter = adapter;
        }

        public async Task<Guid> CreateCommandRegister(int userId, Guid deviceGuid, string methodName, CancellationToken cancellationToken = default)
        {
            var device = await _deviceService.GetDeviceWithIdByGuid(deviceGuid, userId, cancellationToken);
            var deviceMethod = device.Methods?.FirstOrDefault(m => string.Equals(m.MethodName, methodName, StringComparison.OrdinalIgnoreCase));
            if (deviceMethod == null)
            {
                throw new Exception("Method record not found.");
            }
            MethodRegister newMethod = new MethodRegister
            {
                MethodName = methodName,
                UserId = userId,
                DeviceId = device.Id,
                Status = RequestStatus.Pending,
                MethodId = deviceMethod.Id
            };
            var methdRegisterDto = await ProcessMethodRegisterAsync(newMethod, cancellationToken: cancellationToken);
            return methdRegisterDto.CorrelationId;
        }

        public async Task<MethodRegisterResultDto> GetFirstMethodRegisterByDeviceAsync(int userId, Guid deviceGuid, string methodName, CancellationToken cancellationToken = default)
        {
            var methodRegisterRepository = await _unitOfWork.GetRepository<MethodRegister>();
            var device = await _deviceService.GetDeviceWithIdByGuid(deviceGuid, userId, cancellationToken);

            var methodRegisterResults = await methodRegisterRepository.ReadEntitiesByPredicate((elem) => elem.DeviceId == device.Id && elem.UserId == userId && elem.MethodName == methodName,
                                                                                 orderBy: [new KeyValuePair<Expression<Func<MethodRegister, object>>, bool>(elem => elem.RequestTime, true)]);
            var result = methodRegisterResults.LastOrDefault();
            if (result == null)
            {
                throw new Exception("Method record not found.");
            }
            return await _adapter.GetMethodRegisterResultDto(result);
        }

        public async Task UpdateMethodResponseAsync(HandleDeviceResponseDto responseDto, CancellationToken cancellationToken = default)
        {
            var methodRegisterRepository = await _unitOfWork.GetRepository<MethodRegister>();
            var methodRegisters = await methodRegisterRepository.ReadEntitiesByPredicate(m => m.CorrelationId == responseDto.CorrelationId);

            var methodRegister = methodRegisters.FirstOrDefault();
            if (methodRegister == null)
            {
                throw new Exception("Method register record not found.");
            }

            methodRegister.Status = responseDto.Success ? RequestStatus.Ok : RequestStatus.Error;
            methodRegister.Message = responseDto.Message;
            methodRegister.ResponseReceivedAt = DateTime.UtcNow;
            methodRegister.Result = responseDto.Result;

            await methodRegisterRepository.UpdateAsync(methodRegister, cancellationToken);
            await _unitOfWork.CommitAsync();
        }

        private async Task<MethodRegisterDto> ProcessMethodRegisterAsync(MethodRegister methodRegister, CancellationToken cancellationToken = default)
        {
            var methodRegisterRepository = await _unitOfWork.GetRepository<MethodRegister>();

            if (methodRegister.Id == 0)
            {
                await methodRegisterRepository.CreateAsync(methodRegister, cancellationToken: cancellationToken);
            }
            else
            {
                await methodRegisterRepository.UpdateAsync(methodRegister, cancellationToken: cancellationToken);
            }
            await _unitOfWork.CommitAsync();

            return new MethodRegisterDto()
            {
                CorrelationId = methodRegister.CorrelationId,
                MethodName = methodRegister.MethodName,
                Status = methodRegister.Status,
                Message = methodRegister.Message,
                RequestTime = methodRegister.RequestTime
            };
        }
    }
}
