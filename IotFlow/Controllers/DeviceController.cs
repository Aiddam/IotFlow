﻿using IotFlow.Abstractions.Interfaces.Services;
using IotFlow.Models.DTO.Devices;
using IotFlow.Models.DTO.Devices.Register;
using IotFlow.Models.DTO.Devices.Send;
using IotFlow.Models.DTO.Devices.Update;
using IotFlow.Models.DTO.MethodRegister;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IotFlow.Controllers
{
    [Route("api/[controller]s")]
    public class DeviceController : BaseController
    {
        private readonly IDeviceService<DeviceDto, DeviceWithIdDto, RegisterDeviceDto, UpdateDeviceDto, MethodDto, DeviceAliveDto, SendMethodParameterDto> _deviceService;
        private readonly IMethodRegisterService<HandleDeviceResponseDto, MethodRegisterResultDto> _methodRegisterService;
        public DeviceController(IDeviceService<DeviceDto, DeviceWithIdDto, RegisterDeviceDto, UpdateDeviceDto, MethodDto, DeviceAliveDto, SendMethodParameterDto> deviceService, IMethodRegisterService<HandleDeviceResponseDto, MethodRegisterResultDto> methodRegisterService)
        {
            _deviceService = deviceService;
            _methodRegisterService = methodRegisterService;
        }
        [Authorize, HttpGet("{deviceGuid}")]
        public async Task<IActionResult> GetDevice(Guid deviceGuid, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            try
            {
                var device = await _deviceService.GetDeviceByGuid(deviceGuid, userId, cancellationToken);
                return Ok(device);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize, HttpGet("{deviceGuid}/exist")]
        public async Task<IActionResult> IsExist(Guid deviceGuid, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            try
            {
                var device = await _deviceService.GetDeviceByGuid(deviceGuid, userId, cancellationToken);
                if(device != null)
                {
                    return Ok(true);
                }
                return Ok(false);
            }
            catch (Exception ex)
            {
                return Ok(false);
            }
        }
        [Authorize, HttpGet]
        public async Task<IActionResult> GetDevices(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            try
            {
                var devices = await _deviceService.GetDevices(userId, cancellationToken);
                return Ok(devices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize, HttpPost("create")]
        public async Task<ActionResult<DeviceDto>> CreateDevice([FromBody] RegisterDevice registerDevice, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            RegisterDeviceDto registerDeviceDto = new RegisterDeviceDto()
            {
                UserId = userId,
                Name = registerDevice.Name
            };
            DeviceDto device = await _deviceService.CreateDevice(registerDeviceDto, cancellationToken);

            return Ok(device);
        }
        [Authorize, HttpPut("{deviceGuid}")]
        public async Task<ActionResult<UpdateDeviceDto>> UpdateDevice(Guid deviceGuid, [FromBody] UpdateDevice updateDevice, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            UpdateDeviceDto updateDeviceDto = new UpdateDeviceDto
            {
                DeviceGuid = deviceGuid,
                UserId = userId,
                Name = updateDevice.Name
            };

            try
            {
                DeviceDto device = await _deviceService.UpdateDevice(updateDeviceDto, cancellationToken);
                return Ok(device);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize, HttpPut("{deviceGuid}/set-device-alive")]
        public async Task<ActionResult<DeviceAliveDto>> SetDeviceAlive(Guid deviceGuid, [FromBody] DeviceAliveDto updateDevice, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            try
            {
                DeviceDto device = await _deviceService.SetDeviceAlive(deviceGuid, userId, updateDevice, cancellationToken);
                return Ok(device);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize, HttpPost("{deviceGuid}/add-methods")]
        public async Task<IActionResult> AddMethods(Guid deviceGuid, [FromBody] List<MethodDto> methods, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            try
            {
                await _deviceService.AddMethods(deviceGuid, userId, methods, cancellationToken);
                return Ok("Methods added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize, HttpDelete("{deviceGuid}")]
        public async Task<IActionResult> DeleteDevice(Guid deviceGuid, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            try
            {
                await _deviceService.DeleteDevice(deviceGuid, userId, cancellationToken);
                return Ok("Device deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize, HttpPost("{deviceGuid}/command/send")]
        public async Task<IActionResult> SendCommand(Guid deviceGuid, [FromBody] SendMethodDto method, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var correlationId = await _methodRegisterService.CreateCommandRegister(userId, deviceGuid, method.MethodName, cancellationToken);
            try
            {
                await _deviceService.SendCommandAsync(deviceGuid, userId, method.MethodName, method.Parameters, correlationId, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize, HttpPost("{deviceGuid}/handle-response")]
        public async Task<IActionResult> HandleResponse(Guid deviceGuid, [FromBody] HandleDeviceResponseDto responseDto, CancellationToken cancellationToken)
        {
            try
            {
                await _methodRegisterService.UpdateMethodResponseAsync(responseDto, cancellationToken);
                return Ok("Response handled successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
