using IotFlow.Abstractions.Interfaces.Services;
using IotFlow.Models.DTO.Devices.Register;
using IotFlow.Models.DTO.Devices.Send;
using IotFlow.Models.DTO.Devices.Update;
using IotFlow.Models.DTO.Devices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IotFlow.Models.DTO.MethodRegister;

namespace IotFlow.Controllers
{
    public class MethodRegisterController : BaseController
    {
        private readonly IDeviceService<DeviceDto, DeviceWithIdDto, RegisterDeviceDto, UpdateDeviceDto, MethodDto, DeviceAliveDto, SendMethodParameterDto> _deviceService;
        private readonly IMethodRegisterService<HandleDeviceResponseDto, MethodRegisterResultDto> _methodRegisterService;
        public MethodRegisterController(IDeviceService<DeviceDto, DeviceWithIdDto, RegisterDeviceDto, UpdateDeviceDto, MethodDto, DeviceAliveDto, SendMethodParameterDto> deviceService, IMethodRegisterService<HandleDeviceResponseDto, MethodRegisterResultDto> methodRegisterService)
        {
            _deviceService = deviceService;
            _methodRegisterService = methodRegisterService;
        }
        [Authorize, HttpGet("get-by-device/{deviceGuid}/{methodName}")]
        public async Task<ActionResult<MethodRegisterResultDto>> GetMethodRegisterResult(Guid deviceGuid, string methodName, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            try
            {
                var methodRegisterResult = await _methodRegisterService.GetFirstMethodRegisterByDeviceAsync(userId, deviceGuid, methodName, cancellationToken);
                return Ok(methodRegisterResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
